using Noter.Shared.Data;
using SQLite;
using System;
using System.Collections.Generic;

namespace Noter.Shared.DataAccessLayer
{
    public static class DBAccess
    {
        public static readonly SQLiteConnection Connection = new SQLiteConnection(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "noter.db"));

        public static IEnumerable<Type> GetEntityTypes()
        {
            yield return typeof(Library);
            yield return typeof(Shelf);
            yield return typeof(Book);
            yield return typeof(Page);
            yield return typeof(Line);
        }

        public static Type ParseTableName(string tableName)
        {
            switch (tableName)
            {
                case "Library":
                    return typeof(Library);
                case "Shelf":
                    return typeof(Shelf);
                case "Book":
                    return typeof(Book);
                case "Page":
                    return typeof(Page);
                case "Line":
                    return typeof(Line);
            }

            throw new ArgumentException("Could not find table name " + tableName);
        }

        public static TableMapping GetMapping<T>()
        {
            return Connection.GetMapping<T>();
        }

        public static TableMapping GetMapping(Type type)
        {
            return Connection.GetMapping(type);
        }

        public static void InitializeTables()
        {
            DBTable.Create<Library>(false);
            DBTable.Create<Shelf>(false);
            DBTable.Create<Book>(false);
            DBTable.Create<Page>(false);
            DBTable.Create<Line>(false);
        }

        public static void ResetTables()
        {
            DBTable.Create<Library>(true);
            DBTable.Create<Shelf>(true);
            DBTable.Create<Book>(true);
            DBTable.Create<Page>(true);
            DBTable.Create<Line>(true);
        }

        public static void Execute(string query)
        {
            Connection.Execute(query);
        }

        /*public static TopologyStates GetRegionTopologyState(string regionName)
        {
            int nDates = 0;
            int nLoaded = 0;
            bool needsUpdating = false;

            var region = DBTable.GetFirstOrDefault<Region>(r => r.Name == regionName);

            foreach (var subregion in DBTable.GetAll<Subregion>(s => s.RegionID == region.ID))
            {
                var topologyDate = DBTable.GetFirstOrDefault<TopologyDate>(d => d.SubregionID == subregion.ID);
                nDates++;

                if (topologyDate != null && topologyDate.Loaded)
                {
                    nLoaded++;

                    if (topologyDate.LastUpdatedLocal < topologyDate.LastModified)
                    {
                        needsUpdating = true;
                    }
                }
            }

            if (nLoaded > 0)
            {
                if (nLoaded == nDates)
                {
                    return needsUpdating ? TopologyStates.Old : TopologyStates.Full;
                }
                else
                {
                    return TopologyStates.Partial;
                }
            }
            else
            {
                return TopologyStates.Empty;
            }
        }

        public static TopologyStates GetSubregionTopologyState(string regionName, string subregionName)
        {
            var region = DBTable.GetFirstOrDefault<Region>(r => r.Name == regionName);
            var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

            var topologyDate = DBTable.GetFirstOrDefault<TopologyDate>(d => d.SubregionID == subregion.ID);
            if (topologyDate != null)
            {
                if (topologyDate.Loaded)
                {
                    if (topologyDate.LastUpdatedLocal < topologyDate.LastModified)
                    {
                        return TopologyStates.Old;
                    }
                    else
                    {
                        foreach (var intersection in DBTable.GetAll<Intersection>(i => i.SubregionID == subregion.ID))
                        {
                            if (!DBTable.GetAll<Approach>(a => a.IntersectionID == intersection.ID).Any())
                            {
                                return TopologyStates.Partial;
                            }
                        }

                        return TopologyStates.Full;
                    }
                }
                else
                {
                    return TopologyStates.Empty;
                }
            }
            else
            {
                return TopologyStates.Empty;
            }
        }

        public static void UpdateRegions(PSASupplierMapCollection supplierMapping)
        {
            var regions = new List<Region>();

            foreach (var supplierMap in supplierMapping.Items.Values)
            {
                if (!regions.Any(r => r.Name == supplierMap.DeploymentRegion.Name))
                {
                    var region = new Region()
                    {
                        Name = supplierMap.DeploymentRegion.Name
                    };

                    regions.Add(region);
                }
            }

            DBTable.InsertUpdateDelete(regions);
        }

        public static void UpdateSubregions(PSASupplierMapCollection supplierMapping)
        {
            var subregions = new List<Subregion>();

            var subregionsByRegion = new Dictionary<string, List<string>>();
            foreach (var supplierMap in supplierMapping.Items.Values)
            {
                var region = DBTable.GetFirstOrDefault<Region>(r => r.Name == supplierMap.DeploymentRegion.Name);
                if (region != null && !subregions.Any(s => s.Name == supplierMap.PSATargets.Region && s.RegionID == region.ID))
                {
                    var subregion = new Subregion()
                    {
                        Name = supplierMap.PSATargets.Region,
                        RegionID = region.ID
                    };

                    subregions.Add(subregion);
                }
            }

            DBTable.InsertUpdateDelete(subregions);
        }

        public static void UpdateHostServers(PSAServerMapCollection serverMaps)
        {
            var servers = new List<HostServer>();

            foreach (var ipAddress in serverMaps.Items.Values.Select(s => s.Connection.IPAddress.Address))
            {
                if (!servers.Any(s => s.IPAddress == ipAddress))
                {
                    var hostServer = new HostServer()
                    {
                        IPAddress = ipAddress
                    };

                    servers.Add(hostServer);
                }
            }

            DBTable.InsertUpdateDelete(servers);
        }

        public static void UpdateIntersections(IEnumerable<Tuple<int, PSACoverageInfoExpandedInfoResponse>> coverageByIDs)
        {
            var intersections = new List<Intersection>();
            var updateSubregions = new List<Subregion>();

            foreach (var coverageByID in coverageByIDs)
            {
                var hostServerID = coverageByID.Item1;
                var response = coverageByID.Item2;

                foreach (var regionIntersection in response.Regions.Items.Values)
                {
                    string subregionName = regionIntersection.Region.Name;

                    var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName);
                    if (subregion != null)
                    {
                        var subregionCenter = regionIntersection.Outline.GetWeightedCenter();
                        subregion.Latitude = subregionCenter.Y;
                        subregion.Longitude = subregionCenter.X;

                        updateSubregions.Add(subregion);

                        foreach (var psaIntersection in regionIntersection.Intersections.Items.Values)
                        {
                            var intersection = new Intersection()
                            {
                                IntersectionNr = psaIntersection.SCNr,
                                Name = psaIntersection.OnStreet + " & " + psaIntersection.AtStreet,
                                Latitude = psaIntersection.Location.Latitude,
                                Longitude = psaIntersection.Location.Longitude,
                                VerificationType = ServerMapHelper.GetVerificationTypeFromVerificationStatus(psaIntersection.VerificationStatus),
                                SubregionID = subregion.ID,
                                HostServerID = hostServerID
                            };

                            intersections.Add(intersection);
                        }
                    }
                }
            }

            DBTable.UpdateAll(updateSubregions);
            DBTable.InsertUpdateDelete(intersections);
        }

        public static void UpdateTopology(string regionName, IEnumerable<PSATopologyResponse> responses)
        {
            var region = DBTable.GetFirstOrDefault<Region>(r => r.Name == regionName);

            UpdateApproaches(region, responses);
            UpdateApproachOutlines(region, responses);
            UpdateSubregionOutlines(region, responses);
            UpdateLanes(region, responses);
            UpdateSignals(region, responses);

            UpdateLocalTopologyDates(region, responses);
        }

        public static void UpdateSubregionOutlines(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertVertices = new List<SubregionVertex>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;

                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);
                var intersectionIDs = DBTable.GetAll<Intersection>(i => i.SubregionID == subregion.ID).ToList().Select(i => i.ID);
                var approachIDs = DBTable.GetAll<Approach>(v => intersectionIDs.Contains(v.IntersectionID)).ToList().Select(a => a.ID);
                var approachVertices = DBTable.GetAll<ApproachVertex>(v => approachIDs.Contains(v.ApproachID)).ToList();

                int order = 1;
                foreach (var point in GPSMath.GetBoundingShape(approachVertices))
                {
                    var vertex = new SubregionVertex()
                    {
                        Latitude = point.Latitude,
                        Longitude = point.Longitude,
                        Order = order,
                        SubregionID = subregion.ID
                    };

                    insertVertices.Add(vertex);
                    order++;
                }
            }

            DBTable.InsertAll(insertVertices);
        }

        public static void UpdateApproaches(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertApproaches = new List<Approach>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;
                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

                foreach (var topoIntersection in response.Region.Intersections.Items.Values)
                {
                    int intersectionNr = topoIntersection.ID;
                    var intersection = DBTable.GetFirstOrDefault<Intersection>(i => i.IntersectionNr == intersectionNr && i.SubregionID == subregion.ID);

                    foreach (var topoApproach in topoIntersection.Approaches.Items.Values)
                    {
                        var approach = new Approach()
                        {
                            ApproachNr = topoApproach.Approach.ID,
                            Direction = topoApproach.Approach.Name,
                            IntersectionID = intersection.ID
                        };

                        insertApproaches.Add(approach);
                    }
                }
            }

            DBTable.InsertAll(insertApproaches);
        }

        public static void UpdateApproachOutlines(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertApproachVertices = new List<ApproachVertex>();
            var insertStopLines = new List<StopLine>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;
                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

                foreach (var topoIntersection in response.Region.Intersections.Items.Values)
                {
                    int intersectionNr = topoIntersection.ID;
                    var intersection = DBTable.GetFirstOrDefault<Intersection>(i => i.IntersectionNr == intersectionNr && i.SubregionID == subregion.ID);

                    foreach (var topoApproach in topoIntersection.Approaches.Items.Values)
                    {
                        int approachNr = topoApproach.Approach.ID;
                        var approach = DBTable.GetFirstOrDefault<Approach>(a => a.ApproachNr == approachNr && a.IntersectionID == intersection.ID);

                        var approachCoords = topoApproach.Approach.ApproachRegionCoords.Items;

                        for (var i = 0; i < approachCoords.Count / 2; i++)
                        {
                            var gpsLocationA = approachCoords[i];
                            var gpsLocationB = approachCoords[approachCoords.Count - (i + 1)];

                            // Get vector that is perpendicular to the point between locations A and B
                            var vector = new Vector(gpsLocationA.Latitude - gpsLocationB.Latitude, -(gpsLocationA.Longitude - gpsLocationB.Longitude));

                            var vertexA = new ApproachVertex()
                            {
                                Latitude = gpsLocationA.Latitude,
                                Longitude = gpsLocationA.Longitude,
                                Bearing = GPSMath.CartesianAngleToBearing(vector.Angle),
                                Order = i + 1,
                                ApproachID = approach.ID
                            };

                            var vertexB = new ApproachVertex()
                            {
                                Latitude = gpsLocationB.Latitude,
                                Longitude = gpsLocationB.Longitude,
                                Bearing = GPSMath.CartesianAngleToBearing(vector.Angle),
                                Order = approachCoords.Count - i,
                                ApproachID = approach.ID
                            };

                            insertApproachVertices.Add(vertexA);
                            insertApproachVertices.Add(vertexB);
                        }

                        var stoplineCoords = topoApproach.Approach.StopLineRegionCoords.Items;

                        // We only care about vertices 1 and 4, as these are the actual endpoints of the stopline
                        if (stoplineCoords.Count == 6)
                        {
                            var stopLineLocationA = stoplineCoords[1];
                            var stopLineLocationB = stoplineCoords[4];

                            var stopLine = new StopLine()
                            {
                                LatitudeA = stopLineLocationA.Latitude,
                                LongitudeA = stopLineLocationA.Longitude,
                                LatitudeB = stopLineLocationB.Latitude,
                                LongitudeB = stopLineLocationB.Longitude,
                                ApproachID = approach.ID
                            };

                            insertStopLines.Add(stopLine);
                        }
                    }
                }
            }

            DBTable.InsertAll(insertApproachVertices);
            DBTable.InsertAll(insertStopLines);
        }

        public static void UpdateLanes(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertLanes = new List<Lane>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;
                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

                foreach (var topoIntersection in response.Region.Intersections.Items.Values)
                {
                    int intersectionNr = topoIntersection.ID;
                    var intersection = DBTable.GetFirstOrDefault<Intersection>(i => i.IntersectionNr == intersectionNr && i.SubregionID == subregion.ID);

                    foreach (var topoApproach in topoIntersection.Approaches.Items.Values)
                    {
                        int approachNr = topoApproach.Approach.ID;
                        var approach = DBTable.GetFirstOrDefault<Approach>(a => a.ApproachNr == approachNr && a.IntersectionID == intersection.ID);

                        foreach (var topoLane in topoApproach.Approach.Lanes.Items.Values)
                        {
                            var lane = new Lane()
                            {
                                LaneNr = topoLane.LaneNumber,
                                Maneuver = TopologyHelper.GetManeuverTypeFromPSAManeuverImage(topoLane.ManeuverImage),
                                SpeedLimit = UnitConversions.MPHToMPS(topoLane.SpeedLimit),
                                ApproachID = approach.ID
                            };

                            insertLanes.Add(lane);
                        }
                    }
                }
            }

            DBTable.InsertAll(insertLanes);
        }

        public static void UpdateSignals(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertSignals = new List<Signal>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;
                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

                foreach (var topoIntersection in response.Region.Intersections.Items.Values)
                {
                    int intersectionNr = topoIntersection.ID;
                    var intersection = DBTable.GetFirstOrDefault<Intersection>(i => i.IntersectionNr == intersectionNr && i.SubregionID == subregion.ID);

                    foreach (var topoApproach in topoIntersection.Approaches.Items.Values)
                    {
                        int approachNr = topoApproach.Approach.ID;
                        var approach = DBTable.GetFirstOrDefault<Approach>(a => a.ApproachNr == approachNr && a.IntersectionID == intersection.ID);

                        foreach (var topoLane in topoApproach.Approach.Lanes.Items.Values)
                        {
                            if (topoLane is PSATopoReferenceLane topoReferenceLane)
                            {
                                int laneNr = topoLane.LaneNumber;
                                var lane = DBTable.GetFirstOrDefault<Lane>(l => l.LaneNr == laneNr && l.ApproachID == approach.ID);

                                if (lane != null)
                                {
                                    var signal = new Signal()
                                    {
                                        HeadNr = topoReferenceLane.SignalHead,
                                        PhaseNr = topoReferenceLane.SignalGroup,
                                        PermissivePhaseNr = topoReferenceLane.PermSignalGroup,
                                        Type = Signal.GetSignalTypeFromSignalConfig(topoReferenceLane.SignalConfig),
                                        LaneID = lane.ID
                                    };

                                    insertSignals.Add(signal);

                                    if (topoReferenceLane.OrSignalHead > 0 || topoReferenceLane.OrSignalGroup > 0)
                                    {
                                        var orSignal = new Signal()
                                        {
                                            HeadNr = topoReferenceLane.OrSignalHead,
                                            PhaseNr = topoReferenceLane.OrSignalGroup,
                                            LaneID = lane.ID
                                        };

                                        insertSignals.Add(orSignal);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            DBTable.InsertAll(insertSignals);
        }

        public static void UpdateLocalTopologyDates(Region region, IEnumerable<PSATopologyResponse> responses)
        {
            var insertDates = new List<TopologyDate>();
            var updateDates = new List<TopologyDate>();

            foreach (var response in responses)
            {
                string subregionName = response.Request.Region;
                var subregion = DBTable.GetFirstOrDefault<Subregion>(s => s.Name == subregionName && s.RegionID == region.ID);

                if (!insertDates.Any(d => d.SubregionID == subregion.ID) && !updateDates.Any(d => d.SubregionID == subregion.ID))
                {
                    var topologyDate = DBTable.GetFirstOrDefault<TopologyDate>(d => d.SubregionID == subregion.ID);
                    if (topologyDate == null)
                    {
                        topologyDate = new TopologyDate()
                        {
                            Loaded = true,
                            LastModified = DateTime.MinValue,
                            LastUpdatedLocal = DateTime.Now,
                            SubregionID = subregion.ID
                        };

                        insertDates.Add(topologyDate);
                    }
                    else
                    {
                        topologyDate.Loaded = true;
                        topologyDate.LastUpdatedLocal = DateTime.Now;

                        updateDates.Add(topologyDate);
                    }
                }
            }

            DBTable.InsertAll(insertDates);
            DBTable.UpdateAll(updateDates);
        }

        public static void InsertTopologyDate(TopologyDate topologyDate)
        {
            var existingEntry = DBTable.GetFirstOrDefault<TopologyDate>(d => d.SubregionID == topologyDate.SubregionID);

            if (existingEntry != null)
            {
                topologyDate.ID = existingEntry.ID;
                topologyDate.Loaded = existingEntry.Loaded;
                topologyDate.LastUpdatedLocal = existingEntry.LastUpdatedLocal;
                DBTable.Update(topologyDate);
            }
            else
            {
                topologyDate.Loaded = false;
                topologyDate.LastUpdatedLocal = DateTime.MinValue;
                DBTable.Insert(topologyDate);
            }
        }

        public static void DeleteTopology(string regionName)
        {
            string subregionVertexQuery = "DELETE FROM " + typeof(SubregionVertex).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + "))";
            Execute(subregionVertexQuery);

            string approachQuery = "DELETE FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + ")))";
            Execute(approachQuery);

            string approachVertexQuery = "DELETE FROM " + typeof(ApproachVertex).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + "))))";
            Execute(approachVertexQuery);

            string stopLineQuery = "DELETE FROM " + typeof(StopLine).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + "))))";
            Execute(stopLineQuery);

            string laneQuery = "DELETE FROM " + typeof(Lane).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + "))))";
            Execute(laneQuery);

            string signalQuery = "DELETE FROM " + typeof(Signal).Name + " WHERE LaneID in ("
                + "SELECT ID FROM " + typeof(Lane).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE RegionID in ("
                + "SELECT ID FROM " + typeof(Region).Name + " WHERE Name = '" + regionName + "'"
                + ")))))";
            Execute(signalQuery);
        }

        public static void DeleteTopology(string regionName, string subregionName)
        {
            string subregionVertexQuery = "DELETE FROM " + typeof(SubregionVertex).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + ")";
            Execute(subregionVertexQuery);

            string approachQuery = "DELETE FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + "))";
            Execute(approachQuery);

            string approachVertexQuery = "DELETE FROM " + typeof(ApproachVertex).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + ")))";
            Execute(approachVertexQuery);

            string stopLineQuery = "DELETE FROM " + typeof(StopLine).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + ")))";
            Execute(stopLineQuery);

            string laneQuery = "DELETE FROM " + typeof(Lane).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + ")))";
            Execute(laneQuery);

            string signalQuery = "DELETE FROM " + typeof(Signal).Name + " WHERE LaneID in ("
                + "SELECT ID FROM " + typeof(Lane).Name + " WHERE ApproachID in ("
                + "SELECT ID FROM " + typeof(Approach).Name + " WHERE IntersectionID in ("
                + "SELECT ID FROM " + typeof(Intersection).Name + " WHERE SubregionID in ("
                + "SELECT ID FROM " + typeof(Subregion).Name + " WHERE Name = '" + subregionName + "'"
                + "))))";
            Execute(signalQuery);
        }*/
    }
}