using Android.App;
using Android.Content;
using Android.Preferences;
using System;

namespace Noter.Droid.Helpers
{
    public static class PreferencesHelper
    {
        public static ISharedPreferences Preferences => PreferenceManager.GetDefaultSharedPreferences(Application.Context);

        public static void ResetToDefaults() => Preferences.Edit()
            .Clear()
            .Commit();

        public static int PredictionsThreshold
        {
            get
            {
                return int.Parse(Preferences.GetString("predictions_timestamp_threshold", "5"));
            }
        }

        public static int ConfidenceThreshold
        {
            get
            {
                return int.Parse(Preferences.GetString("confidence_threshold", "6"));
            }
        }

        public static bool RoundGLOSA
        {
            get
            {
                return Preferences.GetBoolean("round_glosa", true);
            }
        }

        public static int GLOSABuffer
        {
            get
            {
                return int.Parse(Preferences.GetString("glosa_buffer", "5"));
            }
        }

        public static bool LogDuringTrip
        {
            get
            {
                return Preferences.GetBoolean("trip_log", true);
            }
        }

        public static bool DeveloperDisplay
        {
            get
            {
                return Preferences.GetBoolean("developer_display", false);
            }
        }

        public static bool AlwaysMatch
        {
            get
            {
                return Preferences.GetBoolean("always_match", false);
            }
        }

        public static int MaxBearingAngle
        {
            get
            {
                return Preferences.GetInt("max_bearing_angle", 30);
            }
        }

        public static int TripUpdateInterval
        {
            get
            {
                return int.Parse(Preferences.GetString("trip_update_interval", "1000"));
            }
        }

        public static float TripDistanceInterval
        {
            get
            {
                return float.Parse(Preferences.GetString("trip_smallest_displacement", "1.0"));
            }
        }

        public static int MaxIntersectionDistance
        {
            get
            {
                return int.Parse(Preferences.GetString("max_intersection_distance", "1200"));
            }
        }

        public static int MaxApproachDistance
        {
            get
            {
                return int.Parse(Preferences.GetString("max_approach_distance", "50"));
            }
        }

        public static TimeSpan SupplierMapTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("supplier_map_timeout", "5"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan ServerMapTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("server_map_timeout", "5"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan CoverageTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("coverage_timeout", "30"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan TopologyDateTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("topology_date_timeout", "30"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan TopologyTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("topology_timeout", "60"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan PredictionTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("prediction_timeout", "5"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }

        public static TimeSpan TimingPlanTimeout
        {
            get
            {
                var nSeconds = int.Parse(Preferences.GetString("timing_plan_timeout", "30"));
                return TimeSpan.FromSeconds(nSeconds);
            }
        }
    }
}

/*
  <Preference
          android:key="reset_defaults"
          android:title="Reset to default values"
          android:summary="This will reset ALL settings to the default values"/>*/
