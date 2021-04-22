using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SPCA_Capstone
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string chartData;

        // This method runs upon the page loading or refreshing
        protected void Page_Load(object sender, EventArgs e)
        {
            GetChartData();
        }

        // When a new dog is selected to the dropdown, grab the name of the dog selected and pass it to Load_Dog
        // Load_Dog will fill all the fields with the selected dog's information
        protected void Load_Dog_Click(object sender, EventArgs e)
        {
            string selectedDogName = DogDropdown.SelectedValue;
            // Index 0 is the default value that asks the user to select a dog
            // So, when selected, no data should be shown
            if (DogDropdown.SelectedValue == "Select Dog")
            {
                Set_All_Blank();
            }
            else
            {
                Load_Dog(selectedDogName);
            }
        }

        public void GetChartData()
        {
            string connstr = WebConfigurationManager.ConnectionStrings["SPCAConnectionString"].ConnectionString;

            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();

            string sql = "SELECT Head, Time_Stamp FROM Movement WHERE Dog_ID=" + Get_Dog_ID(DogDropdown.SelectedValue);

            SqlCommand cmd = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            if (dt.Rows.Count == 0)
            {
                chartData = "[[" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ",0]]";
            }
            else
            {
                chartData = "[";
                foreach (DataRow dr in dt.Rows)
                {
                    string sqltime = dr["Time_Stamp"].ToString();
                    DateTimeOffset time = DateTime.Parse(sqltime);
                    long milliseconds = time.ToUnixTimeMilliseconds();
                    chartData += "[" + milliseconds + ", " + dr["Head"] + "],";
                }
                chartData = chartData.Remove(chartData.Length - 1) + "]";
            }
            conn.Close();
        }

        // On clicking the add button, all fields are emptied and enabled to allow editing
        // Additionally, the dropdown menu and options buttons are hidden to not allow users to interact with them
        // The "save" and "cancel" buttons are also made visible
        protected void Add_Dog_Click(object sender, EventArgs e)
        {
            selectDogArea.Visible = false;
            optionsArea.Visible = false;
            saveCancelArea.Visible = true;
            imageUpload.Visible = true;

            Set_All_Blank();

            Set_All_Disabled(false);

            titleText.InnerText = "Adding New Dog";
        }

        // On clicking the edit button, all fields enabled to allow editing
        // Additionally, the dropdown menu and options buttons are hidden to not allow users to interact with them
        // The "save" and "cancel" buttons are also made visible
        // Unlike add dog, the fields are not emptied to allow for easy changes to existing data
        protected void Edit_Dog_Click(object sender, EventArgs e)
        {
            selectDogArea.Visible = false;
            optionsArea.Visible = false;
            saveCancelArea.Visible = true;
            imageUpload.Visible = true;

            Set_All_Disabled(false);

            titleText.InnerText = "Editing Current Dog";
        }

        // Before this method is called, the user is given a confirmation dialog
        // Upon acceptance, this method will remove the selected dog from the database
        // The fields are then reloaded to show as if no dog has been selected
        protected void Delete_Dog_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection
                (WebConfigurationManager.ConnectionStrings["SPCAConnectionString"].ConnectionString);

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Dogs WHERE Dog_Name=@Name", conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Name", dogName.Value);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // This refreshes the dropdown menu
            DogDropdown.Items.Clear();
            DogDropdown.Items.Add("Select Dog");
            DogDropdown.DataBind();

            DogDropdown.SelectedValue = "Select Dog";
            Set_All_Blank();
        }

        // When the save button is clicked, the page reads all the field data and inserts or updates into the database accordingly
        protected void Save_Dog_Click(object sender, EventArgs e)
        {
            // Connect to SPCA database
            SqlConnection conn = new SqlConnection
                (WebConfigurationManager.ConnectionStrings["SPCAConnectionString"].ConnectionString);
            // SqlDataAdapter is used for inserting, updating, or deleting
            SqlDataAdapter adap = new SqlDataAdapter();

            // Open db connection
            conn.Open();

            // Initialize variable for the query string
            string sql;

            // If we are adding a new dog, insert; else update the dog under the selected value's ID
            if (titleText.InnerText.Equals("Adding New Dog"))
            {
                sql = $"INSERT INTO Dogs (Dog_Name, Kennel_ID, Breed, Age, Weight, Notes) VALUES (@Name, @Kennel, @Breed, @Age, @Weight, @Notes);";
            }
            else if (titleText.InnerText.Equals("Editing Current Dog"))
            {
                int dogID = Get_Dog_ID(DogDropdown.SelectedValue);

                sql = $"UPDATE Dogs SET Dog_Name=@Name, Kennel_ID=@Kennel, Breed=@Breed, Age=@Age, Weight=@Weight, Notes=@Notes WHERE Dog_ID={dogID};";
            }
            else
            {
                return;
            }

            SqlCommand cmd;

            // Create executable SQL command
            using (cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@Name", dogName.Value);
                cmd.Parameters.AddWithValue("@Kennel", int.Parse(dogKennelID.Value));
                cmd.Parameters.AddWithValue("@Breed", dogBreed.Value);
                cmd.Parameters.AddWithValue("@Age", float.Parse(dogAge.Value));
                cmd.Parameters.AddWithValue("@Weight", float.Parse(dogWeight.Value));
                cmd.Parameters.AddWithValue("@Notes", notes.Value);
            }
            // Pass the command to the adapter object
            adap.InsertCommand = cmd;
            // Execute the statement against out database
            adap.InsertCommand.ExecuteNonQuery();

            // If an image has been uploaded, save it
            if (imageUpload.HasFile)
            {
                int dogID = Get_Dog_ID(DogDropdown.SelectedValue);

                byte[] imgBytes;
                using (BinaryReader br = new BinaryReader(imageUpload.PostedFile.InputStream))
                {
                    imgBytes = br.ReadBytes(imageUpload.PostedFile.ContentLength);
                }

                sql = $"UPDATE Dogs SET Picture=@Image WHERE Dog_ID={dogID}";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Image", imgBytes);

                adap.InsertCommand = cmd;
                adap.InsertCommand.ExecuteNonQuery();
            }

            sql = "SELECT Picture FROM Dogs WHERE Dog_ID = " + Get_Dog_ID(DogDropdown.SelectedValue); ;
            cmd = new SqlCommand(sql, conn);

            byte[] dbImage;

            object binaryData = cmd.ExecuteScalar();
            if (!binaryData.Equals(System.DBNull.Value))
            {
                dbImage = (byte[])binaryData;
                string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                dogPic.ImageUrl = "data:image/png;base64," + base64String;
            }
            else
            {
                dogPic.ImageUrl = "/Content/paw_print.png";
            }

            // Dispose all objects
            adap.Dispose();
            cmd.Dispose();
            conn.Close();

            // Now, to leave "editing mode"
            selectDogArea.Visible = true;
            optionsArea.Visible = true;
            saveCancelArea.Visible = false;
            imageUpload.Visible = false;

            Set_All_Disabled(true);

            titleText.InnerText = Title;

            // This refreshes the dropdown menu and resets the selected value to the new/edited dog
            DogDropdown.Items.Clear();
            DogDropdown.Items.Add("Select Dog");
            DogDropdown.DataBind();
            DogDropdown.Items.FindByText(dogName.Value).Selected = true;
        }

        // Before this method is called, the user is given a confirmation dialog
        // This method, upon acceptance, sets the dog area and options visible again and hides the save/cancel buttons
        // The dog that was last in the dropdown is loaded and the fields are disabled again
        protected void Cancel_Button_Click(object sender, EventArgs e)
        {
            selectDogArea.Visible = true;
            optionsArea.Visible = true;
            saveCancelArea.Visible = false;
            imageUpload.Visible = false;

            Set_All_Disabled(true);

            Load_Dog(DogDropdown.SelectedValue);

            titleText.InnerText = Title;
        }

        // Loads all information of dogToLoad from database
        private void Load_Dog(string dogToLoad)
        {
            if (dogToLoad.Equals("Select Dog"))
            {
                Set_All_Blank();
                return;
            }
            // Connect to SPCA database
            SqlConnection conn = new SqlConnection
                (WebConfigurationManager.ConnectionStrings["SPCAConnectionString"].ConnectionString);

            // Open db connection
            conn.Open();

            // Time to load all pertinent info
            // We already have the dog's name
            dogName.Value = dogToLoad;

            // Declare variables for the rest
            int dbDogID = -1;
            int dbKennelID = -1;
            string dbBreed = null;
            Nullable<float> dbAge = null;
            Nullable<float> dbWeight = null;
            string dbNotes = null;

            // Run SQL query
            string sql = "SELECT Kennel_ID, Breed, Weight, Age, Notes, Dog_ID FROM Dogs WHERE Dog_Name='" + dogToLoad + "'";
            // Create Sql Command by combining query and connection strings
            SqlCommand cmd = new SqlCommand(sql, conn);

            // Parse the results of the query
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dbKennelID = reader.GetInt32(0);
                    dbBreed = reader.IsDBNull(1) ? null : reader.GetString(1);
                    if (reader.GetFloat(2) == 0)
                    {
                        dbWeight = null;
                    }
                    else
                    {
                        dbWeight = reader.GetFloat(2);
                    }
                    if (reader.GetFloat(3) == 0)
                    {
                        dbAge = null;
                    }
                    else
                    {
                        dbAge = reader.GetFloat(3);
                    }
                    dbNotes = reader.IsDBNull(4) ? null : reader.GetString(4);
                    dbDogID = reader.GetInt32(5);
                }
                reader.Close();
            }

            // Display received data
            dogKennelID.Value = dbKennelID.ToString();
            dogBreed.Value = dbBreed;
            dogWeight.Value = dbWeight.ToString();
            dogAge.Value = dbAge.ToString();
            notes.Value = dbNotes;

            // Finally, grab image if one exists
            sql = "SELECT Picture FROM Dogs WHERE Dog_ID = " + dbDogID;
            cmd = new SqlCommand(sql, conn);

            byte[] dbImage;

            object binaryData = cmd.ExecuteScalar();
            if (!binaryData.Equals(System.DBNull.Value))
            {
                dbImage = (byte[])binaryData;
                string base64String = Convert.ToBase64String(dbImage, 0, dbImage.Length);
                dogPic.ImageUrl = "data:image/png;base64," + base64String;
            }
            else
            {
                dogPic.ImageUrl = "/Content/paw_print.png";
            }
            
            cmd.Dispose();
            conn.Close();
        }

        // Sets all fields to blank; helpful when creating a new dog or deleting a dog
        private void Set_All_Blank()
        {
            dogName.Value = "";
            dogKennelID.Value = "";
            dogBreed.Value = "";
            dogWeight.Value = "";
            dogAge.Value = "";
            notes.Value = "";
        }

        private void Set_All_Disabled(bool disabled)
        {
            DogDropdown.Enabled = disabled;
            dogName.Disabled = disabled;
            dogKennelID.Disabled = disabled;
            dogBreed.Disabled = disabled;
            dogWeight.Disabled = disabled;
            dogAge.Disabled = disabled;
            notes.Disabled = disabled;
        }


        private int Get_Dog_ID(string dogName)
        {
            // Connect to SPCA database
            SqlConnection conn = new SqlConnection
                (WebConfigurationManager.ConnectionStrings["SPCAConnectionString"].ConnectionString);

            // Open db connection
            conn.Open();

            // Create SQL query string
            string sql = "SELECT Dog_ID FROM Dogs WHERE Dog_Name='" + dogName + "';";

            // Create command
            SqlCommand cmd = new SqlCommand(sql, conn);

            //Initialize variable
            int dogID = -1;

            // Record ID
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dogID = reader.GetInt32(0);
                }
                reader.Close();
            }

            cmd.Dispose();
            conn.Close();

            return dogID;
        }
    }
}