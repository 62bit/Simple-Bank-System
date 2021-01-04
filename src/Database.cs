using System;
using System.Data.SQLite;
using System.IO;

namespace bank_system
{
    class Database
    {
        public string Name { get; private set; }

        private SQLiteConnection connection;
        public SQLiteConnection Connection { 
            get { return this.connection; }
            private set { connection = value;  } 
        }

        public Database(string name)
        {
            this.Name = name;

            if(File.Exists(name))
            {
                Console.WriteLine("Database already created...");
                Connection = new SQLiteConnection("Data Source=" + name + ";Version=3;");
            }
            else
            {
                Console.WriteLine("Creating Database...");
                SQLiteConnection.CreateFile(name);
                Connection = new SQLiteConnection("Data Source=" + name + ";Version=3;");
            }
            
            Connection.Open();

            string sql_query = "CREATE TABLE card (id INTEGER PRIMARY KEY AUTOINCREMENT, number TEXT, pin TEXT, balance INTEGER DEFAULT 0)";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            try { command.ExecuteNonQuery(); } catch (Exception ex) {}
        }

        public void InsertCard(string card_number, string pin) 
        {
            string sql_query = "INSERT INTO card (number, pin) VALUES ('" + card_number+ "','" + pin +  "')";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            command.ExecuteNonQuery();
        }
        public void DeleteCard(string card_number)
        {
            string delete_query = "DELETE FROM card WHERE number='" + card_number+ "';";
            SQLiteCommand command = new SQLiteCommand(delete_query, Connection);
            command = new SQLiteCommand(delete_query, Connection);
            command.ExecuteNonQuery();
        }
        public bool CardExists(string card_number) {
            string sql_query = "SELECT * FROM card " +
                   "WHERE number='" + card_number + "';";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            return reader.HasRows;
        }
        public string GetPinNumber(string card_number)
        {
            string sql_query = "SELECT * FROM card " +
                   "WHERE number='" + card_number + "';";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            return reader.GetString(2);
        }

        public int GetBalance(string card_number)
        {
            string sql_query = "SELECT * FROM card " +
                   "WHERE number='" + card_number + "';";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();
            return reader.GetInt32(3);
        }

        public void SetBalance(string card_number, int balance)
        {
            string sql_query = "UPDATE card " +
                   "SET balance=" + balance.ToString() +
                   " WHERE number='"+card_number+"';";
            SQLiteCommand command = new SQLiteCommand(sql_query, Connection);
            command.ExecuteNonQuery();
        }
    }
}