using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public static class UserAttributes
    {
        public static Dictionary<Attribute, bool> attributes = new Dictionary<Attribute, bool>() 
        {
            {new Attribute("Name","name"),true},
            {new Attribute("First Name","first_name"),true},
            {new Attribute("Middle Name","middle_name"),true},
            {new Attribute("Last Name","last_name"),true},
            {new Attribute("Picture","pic_small"),true},
            {new Attribute("Sex","sex"),true},
            {new Attribute("Profile Update Time","profile_update_time"),true},
            {new Attribute("Locale","locale"),true},
            {new Attribute("Hometown","hometown_location"),false},
            {new Attribute("Current Location","current_location"),false},
            {new Attribute("Birthday","birthday"),false},
            {new Attribute("Timezone","timezone"),false},
            {new Attribute("Religion","religion"),false},            
            {new Attribute("Relationship","relationship_status"),false},
            {new Attribute("Political Views","political"),false},
            {new Attribute("Activities","activities"),false},
            {new Attribute("Interests","interests"),false},
            {new Attribute("Music","music"),false},
            {new Attribute("TV","tv"),false},
            {new Attribute("Movies","movies"),false},
            {new Attribute("Books","books"),false},
            {new Attribute("Quotes","quotes"),false},
            {new Attribute("About Me","about_me"),false},
            {new Attribute("Online Presence","online_presence"),false},            
            {new Attribute("Website","website"),false}                      
        };

        public static Dictionary<string, string> attributeFriendsPermissionMapping = new Dictionary<string, string>()
        {
            {"birthday", "friends_birthday"},
            {"hometown_location","friends_hometown"},
            {"current_location","friends_location"},
            {"religion", "friends_religion_politics"},
            {"relationship_status", "friends_relationships"},
            {"political","friends_religion_politics"},
            {"activities","friends_activities"},
            {"interests","friends_interests"},
            {"music","friends_likes"},
            {"tv","friends_likes"},
            {"movies","friends_likes"},
            {"books","friends_likes"},
            {"quotes","friends_about_me"},
            {"about_me","friends_about_me"},
            {"status","friends_status"},
            {"online_presence","friends_online_presence"},
            {"website","friends_website"},
        };

        public static Dictionary<string, string> attributeUserPermissionMapping = new Dictionary<string, string>()
        {
            {"birthday", "user_birthday"},
            {"hometown_location","user_hometown"},
            {"current_location","user_location"},
            {"religion", "user_religion_politics"},
            {"relationship_status", "user_relationships"},
            {"political","user_religion_politics"},
            {"activities","user_activities"},
            {"interests","user_interests"},
            {"music","user_likes"},
            {"tv","user_likes"},
            {"movies","user_likes"},
            {"books","user_likes"},
            {"quotes","user_about_me"},
            {"about_me","user_about_me"},
            {"status","user_status"},
            {"online_presence","user_online_presence"},
            {"website","user_website"}
        };

        public static string createRequiredPermissionsString
        (
            bool bStatusUpdatesChecked,
            bool bWallPostsChecked,
            bool bAccessGroups
        )
        {
            string permissionsString = "&scope=";

            foreach (KeyValuePair<Attribute, bool> kvp in UserAttributes.attributes)
            {
                if (kvp.Value && UserAttributes.attributeFriendsPermissionMapping.ContainsKey(kvp.Key.value))
                {
                    permissionsString += UserAttributes.attributeFriendsPermissionMapping[kvp.Key.value] + ",";
                }
            }

            if (bStatusUpdatesChecked) permissionsString += "read_stream,";
            if (bWallPostsChecked) permissionsString += "read_stream,";
            if (bAccessGroups) permissionsString += "user_groups,";

            return permissionsString.Remove(permissionsString.Length - 1);
        }
    }

    public struct Attribute
    {
        public string name, value;

        public Attribute(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
