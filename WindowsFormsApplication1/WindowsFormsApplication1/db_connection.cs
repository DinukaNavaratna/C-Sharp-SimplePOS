using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class db_connection
    {
        protected string con_string = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\C#\POS - Assignment\POS\WindowsFormsApplication1\Local_DB\C#_POS.mdf;Integrated Security=True");
        protected string qry2 = ("SELECT TOP(1) Id FROM Users WHERE User_Type=replace ORDER BY 1 DESC"); //signup (get latest ID)
        protected string qry4 = ("SELECT * from Users WHERE User_Name =replace1 AND Password =replace2"); //login (check user existance)
        protected string qry6 = ("INSERT INTO Users VALUES (replace1, replace2, replace3, replace4, replace5, replace6, replace7)"); //signup (register user details)
        protected string qry8 = ("SELECT * from Users WHERE replace1=replace2"); //check whether new user is already registered
        protected string qry10 = ("SELECT * from Users WHERE First_Name=replace1 AND Last_Name=replace2");
        protected string qry12 = ("SELECT * from Items WHERE Id=replace1");
        protected string qry14 = ("SELECT TOP(1) Id from Items ORDER BY 1 DESC"); //item_count
        protected string qry16 = ("SELECT * from Users WHERE User_Name=replace1");
        protected string qry18 = ("INSERT INTO Items (Name, Price, Type, Image) VALUES (replace1, replace2, replace3, replace4)"); //new_item (register item details)
        protected string qry20 = ("SELECT * from Items replace1=replace2"); //check_db_item_name (item already registered?) //item_info
        protected string qry22 = ("INSERT INTO Sales (Description, Total, User_Name, Time, Date) VALUES (replace1, replace2, replace3, replace4, replace5)"); //new_item (register item details)
        protected string qry24 = ("INSERT INTO Daily_Inventory VALUES (replace1, replace2, replace3, replace3)"); //insert_daily_inventory
        protected string qry26 = ("UPDATE Daily_Inventory SET Qty=replace1 WHERE Item_Id=replace2 AND Date=replace3"); //update_daily_inventory

    }
}
