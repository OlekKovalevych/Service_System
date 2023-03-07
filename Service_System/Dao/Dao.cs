using MySql.Data.MySqlClient;
using Service_System.DB;
using Service_System.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_System.DAO
{
    public class Dao
    {
        private static MySqlDataReader DATA_READER;
        private static Dao Instance;

        private Dao() { }

        public static Dao GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Dao();
            }
            return Instance;
        }

        public bool CreateAdmin(string login, string pass)
        {
            string sql = "INSERT INTO admins ( Login, Password,) VALUES (@Login, @Password);";
            MySqlCommand command = new MySqlCommand(sql, DataBaseConnection.GetConection());
            try
            {
                DataBaseConnection.GetConection().Open();
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", pass);
                DATA_READER = command.ExecuteReader();
                DataBaseConnection.GetConection().Close();
                return IsExistAdmin(login, pass);
            }
            catch (Exception)
            {
                DATA_READER.Close();
                DataBaseConnection.GetConection().Close();
                return false;
            }
        }
       
        private bool IsExistAdmin(string login, string pass)
        {
            List<Admin> list = Instance.FindAllAdmin();
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].GetLogin() == login && list[i].GetPassword() == pass)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Admin> FindAllAdmin()
        {
            MySqlCommand command = new MySqlCommand("SELECT Id,Login,Password FROM admins;", DataBaseConnection.GetConection());
            try
            {
                DataBaseConnection.GetConection().Open();
                DATA_READER = command.ExecuteReader();
                List<Admin> list = new List<Admin>();
                while (DATA_READER.Read())
                {
                    
                     list.Add(new Admin(int.Parse(DATA_READER[0].ToString()), DATA_READER[1].ToString(),DATA_READER[3].ToString()));
                }
                DATA_READER.Close();
                DataBaseConnection.GetConection().Close();
                return list;
            }
            catch (Exception)
            {
                DATA_READER.Close();
                DataBaseConnection.GetConection().Close();
                return null;
            }
        }

        /* 
       private const string ConnectionStr = "server=localhost;user=root;database=people;password=Alyakoval;";
       private const string SelectReader = "SELECT name, surname, email, id_readers from people.readers WHERE id_readers=@id;";
       private const string SelectBook = "SELECT autor, book_name, date_rent, id_readers, id_book from people.library WHERE id_book=@id;";
       private const string GET_MAX_ID_READER = "SELECT max(id_readers) FROM people.readers;";
       private const string GET_MAX_ID_BOOK = "SELECT max(id_book) FROM people.library;";
       private static readonly MySqlConnection Connection = new MySqlConnection(ConnectionStr);
       private static DataBase instance;

       private DataBase() { }

       public static DataBase GetInstance()
       {
           if (instance == null)
           {
               instance = new DataBase();
           }
           return instance;
       }

       
       
       public bool UpdateReaders(string name, string surname, string email, int id)
       {
           string sql = "Update people.readers Set name = @name, surname = @surname, email=@email where id_readers = @id;";
           MySqlCommand command = new MySqlCommand(sql, Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@name", name);
               command.Parameters.AddWithValue("@surname", surname);
               command.Parameters.AddWithValue("@email", email);
               command.Parameters.AddWithValue("@id", id);
               DATA_READER = command.ExecuteReader();
               DATA_READER.Close();
               Connection.Close();
               return CheckReader(name, surname, email, id);
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }
      
       public Reader FindByIdReaders(string id)
       {
           MySqlCommand command = new MySqlCommand(SelectReader, Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", id);
               DATA_READER = command.ExecuteReader();
               Reader reader = new Reader();
               while (DATA_READER.Read())
               {
                   reader.Name = DATA_READER[0].ToString();
                   reader.Surname = DATA_READER[1].ToString();
                   reader.Id = int.Parse(DATA_READER[3].ToString());
                   reader.Email = DATA_READER[2].ToString();
                   DATA_READER.Close();
                   Connection.Close();
                   return reader;
               }
               return null;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return null;
           }
       }
      
      
       public List<Library> FindAllBoks()
       {
           MySqlCommand command = new MySqlCommand("SELECT autor, book_name, date_rent, id_readers, id_book from people.library", Connection);
           try
           {
               Connection.Open();
               DATA_READER = command.ExecuteReader();
               Library library = new Library();
               List<Library> list = new List<Library>();
               while (DATA_READER.Read())
               {
                   int? current;
                   try
                   {
                       current = int.Parse(DATA_READER[3].ToString());
                   }
                   catch (Exception)
                   {
                       current = null;
                   }
                   list.Add(new Library { Autor = DATA_READER[0].ToString(), BookName = DATA_READER[1].ToString(), IdReader = current, Id = int.Parse(DATA_READER[4].ToString()), Date_rent = library.Date_rent = DATA_READER[2].ToString() });
               }
               DATA_READER.Close();
               Connection.Close();
               return list;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return new List<Library>();
           }
       }
       public bool Delete(string id, string doCase)
       {
           string sql;
           MySqlCommand command;
           try
           {
               switch (doCase)
               {
                   case "book":
                       if (FindByIdBook(id) == null)
                       {
                           return false;
                       }
                       Connection.Open();
                       sql = "DELETE FROM people.library WHERE id_book = @id;";
                       command = new MySqlCommand(sql, Connection);
                       command.Parameters.AddWithValue("@id", id);
                       DATA_READER = command.ExecuteReader();
                       DATA_READER.Close();
                       Connection.Close();
                       return CheckId(id, "SELECT id_book from people.library WHERE id_book=@id;");
                   case "reader":
                       if (FindByIdReaders(id) == null)
                       {
                           return false;
                       }
                       Connection.Open();
                       sql = "DELETE FROM people.readers WHERE id_readers = @id;";
                       command = new MySqlCommand(sql, Connection);
                       command.Parameters.AddWithValue("@id", id);
                       DATA_READER = command.ExecuteReader();
                       DATA_READER.Close();
                       Connection.Close();
                       return CheckId(id, "SELECT  id_readers from people.readers WHERE id_readers=@id;");
                   default:
                       return false;
               }
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }

       public bool rentBook(int idReader, int idBook, string date)
       {
           MySqlCommand command = new MySqlCommand("UPDATE people.library SET date_rent=@date, id_readers=@idReaders WHERE id_book=@id;", Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", idBook);
               command.Parameters.AddWithValue("@idReaders", idReader);
               command.Parameters.AddWithValue("@date", date);
               DATA_READER = command.ExecuteReader();
               DATA_READER.Close();
               Connection.Close();
               Library library = FindByIdBook(idBook.ToString());
               return library.IdReader == idReader && library.Id == idBook;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }

       public bool DeleteRent(int idBook)
       {
           MySqlCommand command = new MySqlCommand("UPDATE people.library SET date_rent=null, id_readers=null WHERE id_book = @Id; ", Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", idBook);
               DATA_READER = command.ExecuteReader();
               DATA_READER.Close();
               Connection.Close();
               Library library = FindByIdBook(idBook.ToString());
               return library.IdReader == null && library.Date_rent == "";
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }
       private bool CheckId(string id, string sql)
       {
           bool isDelete = true;
           MySqlCommand command = new MySqlCommand(sql, Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", id);
               DATA_READER = command.ExecuteReader();
               while (DATA_READER.Read())
               {
                   isDelete = false;
               }
               DATA_READER.Close();
               Connection.Close();
               return isDelete;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }

       private bool CheckReader(string name, string surneme, string email, int id)
       {
           bool isUpdate = false;
           MySqlCommand command = new MySqlCommand(SelectReader, Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", id);
               DATA_READER = command.ExecuteReader();
               while (DATA_READER.Read())
               {
                   if (name == DATA_READER[0].ToString() && surneme == DATA_READER[1].ToString() && email == DATA_READER[2].ToString())
                   {
                       isUpdate = true;
                   }
               }
               DATA_READER.Close();
               Connection.Close();
               return isUpdate;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }
       }
       private bool CheckUpdateBooks(string autor, string bookName, int id_book)
       {
           bool isUpdate = false;
           MySqlCommand command = new MySqlCommand(SelectBook, Connection);
           try
           {
               Connection.Open();
               command.Parameters.AddWithValue("@id", id_book);
               DATA_READER = command.ExecuteReader();
               while (DATA_READER.Read())
               {
                   if (autor == DATA_READER[0].ToString() && bookName == DATA_READER[1].ToString())
                   {
                       isUpdate = true;
                   }
               }
               DATA_READER.Close();
               Connection.Close();
               return isUpdate;
           }
           catch (Exception)
           {
               DATA_READER.Close();
               Connection.Close();
               return false;
           }

       }
       

       private int getId(string sql)
       {
           Connection.Open();
           MySqlCommand command = new MySqlCommand(sql, Connection);
           DATA_READER = command.ExecuteReader();
           DATA_READER.Read();
           int i = int.Parse(DATA_READER[0].ToString());
           DATA_READER.Close();
           Connection.Close();
           return i;
       }
   }*/
    }
}
