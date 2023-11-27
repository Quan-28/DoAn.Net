using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace petStore
{
    internal class ConnectData : DataTable
    {
        // khai báo biến
        public SqlConnection connection;
        SqlDataAdapter adapter;
        SqlCommand command;
        #region lấy chuỗi kết nối và mở kết nối
        // lấy chuỗi kết nối
        public string ConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Server"] = "DESKTOP-84E9SG2\\SQLEXPRESS";
            builder["Database"] = "QLPetShop";
            builder["Integrated Security"] = "True";
            
            //builder["Uid"] = "sa";
            //builder["Pwd"] = "123456";
            return builder.ConnectionString;
        }
        // mở kêt nối
        public bool OpenConnection()
        {
            try
            {
                if (connection == null)
                    connection = new SqlConnection(ConnectionString());
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                return true;
            }
            catch
            {
                connection.Close();
                return false;
            }
        }
        #endregion
        #region Thực thi câu lệnh Select
        // Thực thi câu lệnh Select
        public void Fill(SqlCommand selectCommand)
        {
            command = selectCommand;
            try
            {
                command.Connection = connection;
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                this.Clear();
                adapter.Fill(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thực thi câu lệnh SQL này!\nLỗi: " +
                ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Thực thi câu lệnh Insert, Update, Delete
        // Thực thi câu lệnh Insert, Update, Delete
        public int Update()
        {
            int result = 0;
            SqlTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;
                adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                result = adapter.Update(this);
                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();
                MessageBox.Show("Không thể thực thi câu lệnh SQL này!\nLỗi: " +
                e.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        public int Update(SqlCommand insertUpdateDeleteCommand)
        {
            int result = 0;
            SqlTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();
                insertUpdateDeleteCommand.Connection = connection;
                insertUpdateDeleteCommand.Transaction = transaction;
                result = insertUpdateDeleteCommand.ExecuteNonQuery();
                this.AcceptChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    transaction.Rollback();
                MessageBox.Show("Không thể thực thi câu lệnh SQL này!\nLỗi: " +
                e.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        #endregion
    }
}
