using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Smrf.AppLib
{
    public static class AttributeUtils
    {
        public struct Attribute
        {
            public string name, value;

            public Attribute(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        public static List<Attribute> UserAttributes = new List<Attribute>()
        {
            new Attribute("Name","name"),
            new Attribute("First Name","first_name"),
            new Attribute("Middle Name","middle_name"),
            new Attribute("Last Name","last_name"),
            new Attribute("Hometown","hometown_location"),
            new Attribute("Current Location","current_location"),
            new Attribute("Birthday","birthday"),
            new Attribute("Picture","pic_small"),
            new Attribute("Profile Update Time","profile_update_time"),
            new Attribute("Timezone","timezone"),
            new Attribute("Religion","religion"),
            new Attribute("Sex","sex"),
            new Attribute("Relationship","relationship_status"),
            new Attribute("Political Views","political"),
            new Attribute("Activities","activities"),
            new Attribute("Interests","interests"),
            new Attribute("Music","music"),
            new Attribute("TV","tv"),
            new Attribute("Movies","movies"),
            new Attribute("Books","books"),
            new Attribute("Quotes","quotes"),
            new Attribute("About Me","about_me"),
            new Attribute("Online Presence","online_presence"),
            new Attribute("Locale","locale"),
            new Attribute("Website","website"),
        };

        
        
    }
}
