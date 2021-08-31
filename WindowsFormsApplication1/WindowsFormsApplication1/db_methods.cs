using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication1
{
    class db_methods : db_connection
    {
        public string get_latest_id(string utype)
        {
            string lastid = "Error! Couldn't read the latest ID.";

            SqlConnection con = new SqlConnection(con_string);
            string select = qry2.Replace("replace", "'" + utype + "'");
            SqlCommand cmd = new SqlCommand(select, con);
            con.Open();
            SqlDataReader last_id = cmd.ExecuteReader();
            while (last_id.Read())
            {
                lastid = last_id[0].ToString();
            }
            return lastid;
        }

        public Tuple<string, int> log_in(string username, string password, int wrong_psw_count)
        {
            string message = "Error!";
            SqlConnection con = new SqlConnection(con_string);
            string select = qry4
                .Replace("replace1", "'" + username + "'")
                .Replace("replace2", "'" + password + "'");
            SqlCommand cmd = new SqlCommand(select, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter reader = new SqlDataAdapter(cmd);
                reader.Fill(dt);
                try
                {
                    if (dt.Rows[0][0].ToString() != "0")
                    {
                        message = ("Successfully Logged In!");
                        return new Tuple<string, int>(message, 0);
                    }
                    else
                    {
                        if (wrong_psw_count < 3)
                        {
                            message = ("Wrong Username/ Password Combination!");
                            return new Tuple<string, int>(message, wrong_psw_count);
                        }
                        wrong_psw_count = wrong_psw_count + 1;
                    }
                }
                catch (Exception ex)
                {
                    if (wrong_psw_count < 3)
                    {
                        message = ("Wrong Username/ Password Combination!");
                        return new Tuple<string, int>(message, wrong_psw_count);
                    }
                    wrong_psw_count = wrong_psw_count + 1;
                }
            }
            catch (SqlException se)
            {
                message = ("" + se);
                return new Tuple<string, int>(message, wrong_psw_count);
            }
            return new Tuple<string, int>(message, wrong_psw_count);
        }

        public Tuple<string, int> signup(int Id, string User_Name, string Password, string User_Type, string First_Name, string Last_Name, string Email)
        {
            string message = "No Proceed!";
            int new_user = 0;

            new_user = check_db_username(User_Name, Email, First_Name, Last_Name);

            if (new_user == 1)
            {
                SqlConnection con = new SqlConnection(con_string);
                string select = qry6
                    .Replace("replace1", "'" + Id + "'")
                    .Replace("replace2", "'" + User_Name + "'")
                    .Replace("replace3", "'" + Password + "'")
                    .Replace("replace4", "'" + User_Type + "'")
                    .Replace("replace5", "'" + First_Name + "'")
                    .Replace("replace6", "'" + Last_Name + "'")
                    .Replace("replace7", "'" + Email + "'");
                SqlCommand cmd = new SqlCommand(select, con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    message = "User registered successfully!";
                }
                catch (Exception x)
                {
                    message = x.ToString();
                }
            }
            else if (new_user == 2)
            {
                message = "There's an account already registered with the same username\nPlease choose a different username.";
            }
            else if (new_user == 3)
            {
                message = "There's an account already registered with the same email!\nPlease choose a different username.";
            }
            else if (new_user == 4)
            {
                message = "'" + First_Name + " " + Last_Name + "'" + " already has an registered in the system.";
            }
            else
            {
                message = "Error : " + new_user;
            }
            return new Tuple<string, int>(message, new_user);
        }

        private int check_db_username(string uname, string email, string fname, string lname)
        {
            int new_user = 0;
            SqlConnection con = new SqlConnection(con_string);
            string qry = qry8
                .Replace("replace1", "User_Name")
                .Replace("replace2", "'" + uname + "'");
            SqlCommand cmd = new SqlCommand(qry, con);

            con.Open();
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter reader = new SqlDataAdapter(cmd);
            reader.Fill(dt);
            try
            {
                if (dt.Rows[0][0].ToString() != "0")
                {
                    new_user = 2;
                }
                else
                {
                    new_user = check_db_email(email, fname, lname);
                }
            }
            catch (Exception x)
            {
                new_user = check_db_email(email, fname, lname);
            }
            return new_user;
        }

        private int check_db_email(string email, string fname, string lname)
        {
            int new_user = 0;
            SqlConnection con = new SqlConnection(con_string);
            string qry = qry8
                .Replace("replace1", "Email")
                .Replace("replace2", "'" + email + "'");
            SqlCommand cmd = new SqlCommand(qry, con);

            con.Open();
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter reader = new SqlDataAdapter(cmd);
            reader.Fill(dt);
            try
            {
                if (dt.Rows[0][0].ToString() != "0")
                {
                    new_user = 3;
                }
                else
                {
                    new_user = check_db_name(fname, lname);
                }
            }
            catch (Exception x)
            {
                new_user = check_db_name(fname, lname);
            }
            return new_user;
        }

        private int check_db_name(string fname, string lname)
        {
            int new_user = 0;
            SqlConnection con = new SqlConnection(con_string);
            string qry = qry10
                .Replace("replace1", "'" + fname + "'")
                .Replace("replace2", "'" + lname + "'");
            SqlCommand cmd = new SqlCommand(qry, con);

            con.Open();
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter reader = new SqlDataAdapter(cmd);
            reader.Fill(dt);
            try
            {
                if (dt.Rows[0][0].ToString() != "0")
                {
                    new_user = 4;
                }
                else
                {
                    new_user = 1;
                }
            }
            catch (Exception x)
            {
                new_user = 1;
            }
            return new_user;
        }

        public Tuple<string, string, string, int> forgot_psw(string username)
        {
            string message_email = "";
            string message_userfound = "";
            string message_exception = "";
            int message_usertype = 0;

            SqlConnection con = new SqlConnection(con_string);
            string select = "SELECT * from Users WHERE User_Name ='" + username + "'";
            SqlCommand cmd = new SqlCommand(select, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter reader = new SqlDataAdapter(cmd);
                reader.Fill(dt);
                try
                {
                    if (dt.Rows[0][0].ToString() != "0")
                    {
                        message_userfound = "User Found!";

                        string find = "SELECT User_Type from Users WHERE User_Name ='" + username + "'";
                        SqlCommand cmd1 = new SqlCommand(find, con);
                        SqlDataReader uid = cmd1.ExecuteReader();
                        while (uid.Read())
                        {
                            string usertype = Convert.ToString(uid[0]);
                            uid.Close();

                            message_usertype = string.Compare(usertype, "User");

                            if (message_usertype == 1)
                            {
                                con.Close();
                            }
                            else
                            {
                                try
                                {
                                    SqlCommand command = new SqlCommand("SELECT * from Users WHERE User_Name ='" + username + "'", con);
                                    string psw = "";
                                    string email = "";
                                    using (var dr = command.ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            psw = dr["Password"].ToString();
                                            email = dr["Email"].ToString();
                                        }
                                    }

                                    message_email = admin_psw_email(email, username, psw);


                                }
                                catch (Exception ex)
                                {
                                    message_email = "Error Sending Email! \n\n" + ex.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        message_userfound = "User not Found!";
                    }
                }
                catch (Exception ex1)
                {
                    if (!(message_userfound.Equals("User Found!")))
                    {
                        message_userfound = "User not Found!";
                    }
                }
            }
            catch (SqlException se)
            {
                message_exception = "Error! Please take a snap of this error message and send to the developer!\n\n" + se;
            }

            return new Tuple<string, string, string, int>(message_email, message_userfound, message_exception, message_usertype);
        }

        private string admin_psw_email(string email, string username, string psw)
        {
            MailMessage message = new MailMessage();
            SmtpClient mail = new SmtpClient();
            message.From = new MailAddress("pos@infotechdesigners.com", "POS - INFOTECH Designers");
            message.To.Add(new MailAddress(email));
            message.Subject = (username + " POS Login Details.");
            message.IsBodyHtml = true;                                      //string to html  
            message.Body = (CreateBody(username, psw));
            mail.Port = 25;
            mail.Host = "mail.infotechdesigners.com";                       //email host  
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            mail.Credentials = new NetworkCredential("pos@infotechdesigners.com", "posinfotechdesigners");
            mail.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail.Send(message);
            return "An email is sent to the user of this admin account!\n" + email;
        }
        private string CreateBody(string uname, string pswd)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(@"E:\C#\POS - Assignment\POS\WindowsFormsApplication1\WindowsFormsApplication1\HTMLPage1.html"))
            {
                body = reader.ReadToEnd();
                body = body.Replace("{uname}", uname);
                body = body.Replace("{pswd}", pswd);
            }
            return body;
        }


        public Tuple<string, string> forgot_username(string email)
        {
            string message_userfound = "";
            string message_username = "";

            SqlConnection con = new SqlConnection(con_string);
            string select = "SELECT * from Users WHERE Email ='" + email + "'";
            SqlCommand cmd = new SqlCommand(select, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter reader = new SqlDataAdapter(cmd);
                reader.Fill(dt);
                try
                {
                    if (dt.Rows[0][0].ToString() != "0")
                    {
                        message_userfound = "User Found!";

                        string find = "SELECT User_Name from Users WHERE Email ='" + email + "'";
                        SqlCommand cmd1 = new SqlCommand(find, con);
                        using (SqlDataReader uid = cmd1.ExecuteReader())
                            while (uid.Read())
                            {
                                message_username = "Username : " + Convert.ToString(uid[0]);
                                uid.Close();
                            }

                    }
                    else
                    {
                        message_userfound = "User not found!";
                    }
                }
                catch (Exception x)
                {
                    if (!(message_userfound.Equals("User Found!")))
                    {
                        message_userfound = "User not found!";
                    }
                }
            }
            catch (Exception x)
            {

            }
            return new Tuple<string, string>(message_userfound, message_username);
        }


        public Tuple<string, string, string> user_password_reset(string admin_username, string admin_password, string new_password, string confirm_password, string username)
        {
            string message_success = "";
            string message_error = "";
            string message_error2 = "";
            string message_error3 = "";

            SqlConnection con = new SqlConnection(con_string);
            string select = "SELECT * from Users WHERE User_Name ='" + admin_username + "' AND Password ='" + admin_password + "' AND User_Type = 'Admin'";
            SqlCommand cmd = new SqlCommand(select, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter reader = new SqlDataAdapter(cmd);
                reader.Fill(dt);
                try
                {
                    if (dt.Rows[0][0].ToString() != "0")
                    {
                        if (new_password == confirm_password)
                        {
                            SqlConnection con1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\C#\POS - Assignment\POS\WindowsFormsApplication1\Local_DB\C#_POS.mdf;Integrated Security=True");
                            string select1 = "UPDATE Users SET Password ='" + new_password + "' WHERE User_Name ='" + username + "'";
                            SqlCommand cmd1 = new SqlCommand(select1, con);
                            try
                            {
                                con1.Open();
                                cmd1.ExecuteNonQuery();
                                message_success = "'" + username + "' password changed successfully!";

                            }
                            catch (SqlException e1)
                            {
                                message_error = "Connection Error!";
                            }
                        }
                        else
                        {
                            message_success = "Passwords not matching!";
                        }

                    }
                    else
                    {
                        message_success = "Admin not Found!";
                    }

                }
                catch (Exception ex2)
                {
                    message_error2 = "Admin not Found!\nPlease check your username & password and try again.";
                }
            }
            catch (Exception ex3)
            {
                message_error = "Error! Please take a snap of this error message and send to the developer!\n\n" + ex3;
            }

            return new Tuple<string, string, string>(message_success, message_error, message_error2);
        }

        public Tuple<string, double> item_info_for_bill(int id)
        {
            string name = "";
            double price = 0;
            int item_id = 0;

            SqlConnection con = new SqlConnection(con_string);
            string select = qry12
                .Replace("replace1", "'" + id + "'");
            SqlCommand cmd = new SqlCommand(select, con);

            try
            {
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();

                name = sdr["Name"].ToString();
                price = double.Parse(sdr["Price"].ToString());
                item_id = int.Parse(sdr["Id"].ToString());
            }
            catch (SqlException se)
            {
                name = "Error!";
            }

            return new Tuple<string, double>(name, price);
        }

        public int item_count()
        {
            int item_count = 0;

            SqlConnection con = new SqlConnection(con_string);
            string Iteam_cnt = qry14;
            SqlCommand cmd = new SqlCommand(Iteam_cnt, con);

            try
            {
                con.Open();
                SqlDataReader item_cout = cmd.ExecuteReader();
                item_cout.Read();
                item_count = int.Parse(item_cout["Id"].ToString());
            }
            catch (SqlException se)
            {
            }
            return item_count;
        }

        public string if_admin(string username)
        {
            string un = "";

            string name = "";
            double price = 0;

            SqlConnection con = new SqlConnection(con_string);
            string Iteam_cnt = qry16
                .Replace("replace1", "'" + username + "'");
            SqlCommand cmd = new SqlCommand(Iteam_cnt, con);

            try
            {
                con.Open();
                SqlDataReader item_cout = cmd.ExecuteReader();
                item_cout.Read();
                un = item_cout["User_Type"].ToString();
            }
            catch (SqlException se)
            {

            }
            return un;
        }

        public Tuple<string, int> new_item(string Name, double Price, string Type, string Image)
        {
            string message = "No Proceed!";
            int new_item = 0;

            new_item = check_db_item_name(Name);

            if (new_item == 1)
            {
                SqlConnection con = new SqlConnection(con_string);
                string select = qry18
                    .Replace("replace1", "'" + Name + "'")
                    .Replace("replace2", "'" + Price + "'")
                    .Replace("replace3", "'" + Type + "'")
                    .Replace("replace4", "'" + Image + "'");
                SqlCommand cmd = new SqlCommand(select, con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    message = "Item registered successfully!";
                }
                catch (Exception x)
                {
                    message = x.ToString();
                }
            }
            else if (new_item == 2)
            {
                message = "There's an item already registered with the same name\nPlease choose a different item/name.";
            }
            else
            {
                message = "Error : " + new_item;
            }
            return new Tuple<string,int>(message,new_item);
        }
        private int check_db_item_name(string Name)
        {
            int new_item = 0;
            SqlConnection con = new SqlConnection(con_string);
            string qry = qry20
                .Replace("replace1", "WHERE Name")
                .Replace("replace2", "'" + Name + "'");
            SqlCommand cmd = new SqlCommand(qry, con);

            con.Open();
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter reader = new SqlDataAdapter(cmd);
            reader.Fill(dt);
            try
            {
                if (dt.Rows[0][0].ToString() != "0")
                {
                    new_item = 2;
                }
                else
                {
                    new_item = 1;
                }
            }
            catch (Exception x)
            {
                new_item = 1;
            }
            return new_item;
        }

        public string add_bill(string Description, double Total, string User_Name, string Time, string Date)
        {
            string message = "Error Occured!";

                SqlConnection con = new SqlConnection(con_string);
                string select = qry22
                    .Replace("replace1", "'" + Description + "'")
                    .Replace("replace2", "'" + Total + "'")
                    .Replace("replace3", "'" + User_Name + "'")
                    .Replace("replace4", "'" + Time + "'")
                    .Replace("replace5", "'" + Date + "'");
                SqlCommand cmd = new SqlCommand(select, con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    message = "Thankyou!";
                }
                catch (Exception x)
                {
                    message = x.ToString();
                }
            return message;
        }

        public Tuple<int, string, double, string> item_info(int id)
        {
            int item_id = 0;
            string item_name = "";
            double item_price = 0;
            string item_type = "";

            SqlConnection con = new SqlConnection(con_string);
            string items = qry20
                .Replace("replace1", "WHERE Id")
                .Replace("replace2", "'" + id + "'");
            SqlCommand cmd = new SqlCommand(items, con);

            try
            {
                con.Open();
                SqlDataReader item = cmd.ExecuteReader();
                item.Read();

                item_id = int.Parse(item["Id"].ToString());
                item_name = (item["Name"].ToString());
                item_price = double.Parse(item["Price"].ToString());
                item_type = (item["Type"].ToString());
            }
            catch (SqlException se)
            {
            }
            return new Tuple<int,string,double,string>(item_id, item_name, item_price, item_type);
        }

        public string insert_daily_inventory(int item_id, string date, int qty)
        {
            string message = "Error Occured!";

            SqlConnection con = new SqlConnection(con_string);
            string insert = qry24
                .Replace("replace1", "'" + item_id + "'")
                .Replace("replace2", "'" + date + "'")
                .Replace("replace3", "'" + qty + "'");
            SqlCommand cmd = new SqlCommand(insert, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                message = "";
            }
            catch (Exception x)
            {
                message = x.ToString();
            }
            return message;
        }

        public string update_daily_inventory(int item_id, int qty, string date)
        {
            string message = "Error Occured!";

            SqlConnection con = new SqlConnection(con_string);
            string update = qry26
                .Replace("replace1", "'" + qty + "'")
                .Replace("replace2", "'" + item_id + "'")
                .Replace("replace3", "'" + date + "'");
            SqlCommand cmd = new SqlCommand(update, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                message = "Daily Inventory Updated Successfully!";
            }
            catch (Exception x)
            {
                message = x.ToString();
            }
            return message;
        }

        public int user_type(string username)
        {
            int usertype1 = 2;
            SqlConnection con = new SqlConnection(con_string);
            string findtp = "SELECT User_Type from Users WHERE User_Name ='" + username + "'";
            SqlCommand cmdtp = new SqlCommand(findtp, con);

            try
            {
                con.Open();
                SqlDataReader item_cout = cmdtp.ExecuteReader();
                item_cout.Read();
                string usertype = item_cout["User_Type"].ToString();
                usertype1 = string.Compare(usertype, "User");
            }
            catch (SqlException se)
            {

            }
            
            return usertype1;
        }
    }
}
