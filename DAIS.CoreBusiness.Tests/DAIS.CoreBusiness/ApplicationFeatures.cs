using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.CoreBusiness
{
    public class ApplicationFeatures
    {
        public ApplicationFeatures()
        {

        }
        private string userManagement = "User Management";
        private string masters = "Masters";
        private string createUser = "User Creations";
        private string assetOnboarding = "Asset Onboarding";
        private string assetBulkOnboarding = "Asset Bulk Onboarding";
        private string softwareOnboarding = "Software Onboarding";
        private string adminDashboard = "Admin Dashboard";
        private string materialWarranty = "Material Warranty";

        public string UserManagement { get => userManagement; set => userManagement = value; }
        public string Masters { get => masters; set => masters = value; }
        public string CreateUser { get => createUser; set => createUser = value; }
        public string AssetOnboarding { get => assetOnboarding; set => assetOnboarding = value; }
        public string AssetBulkOnboarding { get => assetBulkOnboarding; set => assetBulkOnboarding = value; }
        public string SoftwareOnboarding { get => softwareOnboarding; set => softwareOnboarding = value; }
        public string AdminDashboard { get => adminDashboard; set => adminDashboard = value; }
        public string MaterialWarranty {  get => materialWarranty; set => materialWarranty = value;}
    }


    public static class FeaturePermissions
    {
        public static string Create = "Create";
        public static string Edit = "Edit";
        public static string Delete = "Delete";
        public static string View = "View";
    }
}
