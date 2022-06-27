using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BankOfSLib
{

    public class transactions
    {
        public int accNum { get; set; } 
        public string accType { get; set; }
        public float prevBalance { get; set; }
        public int transNum { get; set; }   
        public string tranType { get; set; }
        public float tranAmount { get; set; }
        public float currBalance { get; set; }  
        public string dateOfTrans { get; set; }

        public int destinAccount { get; set; }
        SqlConnection con = new SqlConnection("server=DESKTOP-2NFCJSD\\STERLINGINSTANCE;database=BankOfSterling;integrated security = true");

        // Admin transfer
        public string accTransfer(int originAccNum, int destinAccNum, int transactionNum,float tranAmount )
        {
            SqlCommand cmdFrom = new SqlCommand("update accounts set accBal = accBal - @amount where accNum = @fromAccount", con);
            cmdFrom.Parameters.AddWithValue("@amount", tranAmount);
            cmdFrom.Parameters.AddWithValue("@fromAccount", originAccNum);

            SqlCommand cmdTo = new SqlCommand("update accounts set accBal = accBal + @amount where accNum = @ToAccount", con);
            cmdTo.Parameters.AddWithValue("@amount", tranAmount);
            cmdTo.Parameters.AddWithValue("@ToAccount", destinAccNum);

            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", originAccNum);
            cmdTransaction.Parameters.AddWithValue("@transNum", transactionNum);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Transfer");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", destinAccNum);
            cmdTransaction.Parameters.AddWithValue("@amount", tranAmount);

            con.Open();
            cmdFrom.ExecuteNonQuery();
            cmdTo.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();

            return "Transfer Complete";
        }

        //Customer Transfer
        public string accTransfer(int originAccNum, int destinAccNum, float tranAmount)
        {
            SqlCommand cmdFrom = new SqlCommand("update accounts set accBal = accBal - @amount where accNum = @fromAccount", con);
            cmdFrom.Parameters.AddWithValue("@amount", tranAmount);
            cmdFrom.Parameters.AddWithValue("@fromAccount", originAccNum);

            SqlCommand cmdTo = new SqlCommand("update accounts set accBal = accBal + @amount where accNum = @ToAccount", con);
            cmdTo.Parameters.AddWithValue("@amount", tranAmount);
            cmdTo.Parameters.AddWithValue("@ToAccount", destinAccNum);

            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", originAccNum);
            cmdTransaction.Parameters.AddWithValue("@transNum", 0);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Transfer");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", destinAccNum);
            cmdTransaction.Parameters.AddWithValue("@amount", tranAmount);

            con.Open();
            cmdFrom.ExecuteNonQuery();
            cmdTo.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();

            return "Transfer Complete";
        }

        public List<transactions>getTransactionHistory(int accountNum)
        {
            List<transactions> shellList = new List<transactions>();
            SqlCommand cmdGatherInfo = new SqlCommand("select * from transactions where accNum = @accountNum", con);
            cmdGatherInfo.Parameters.AddWithValue("@accountNum", accountNum);
    
            //get accounts and their details
            con.Open();
            SqlDataReader readDetail = cmdGatherInfo.ExecuteReader();
            while (readDetail.Read())
            {
                transactions Actions = new transactions();
                Actions.accNum = accountNum;
                Actions.transNum = (int)(readDetail[1]);
                Actions.tranType = (string)readDetail[2];
                Actions.tranAmount = Convert.ToSingle(readDetail[3]);
                Actions.dateOfTrans = (string)readDetail[4];
                Actions.destinAccount = (int)readDetail[5];
                shellList.Add(Actions);

            }

            readDetail.Close();
            con.Close();

            return shellList;
        }

        //Customer withdrawl
        public string preformWithdrawl(int accountNum, float amountTrans)
        {
            SqlCommand cmdFrom = new SqlCommand("update accounts set accBal = accBal - @amount where accNum = @fromAccount", con);
            cmdFrom.Parameters.AddWithValue("@amount", amountTrans);
            cmdFrom.Parameters.AddWithValue("@fromAccount", accountNum);


            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", accountNum);
            cmdTransaction.Parameters.AddWithValue("@transNum", 0);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Withdrawl");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", 0);
            cmdTransaction.Parameters.AddWithValue("@amount", amountTrans);

            con.Open();
            cmdFrom.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();
            return "Withdrawl Successful";
        }

        //Admin Withdrawl
        public string preformWithdrawl(int accountNum, float amountTrans, int transactionNum)
        {
            SqlCommand cmdFrom = new SqlCommand("update accounts set accBal = accBal - @amount where accNum = @fromAccount", con);
            cmdFrom.Parameters.AddWithValue("@amount", amountTrans);
            cmdFrom.Parameters.AddWithValue("@fromAccount", accountNum);


            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", accountNum);
            cmdTransaction.Parameters.AddWithValue("@transNum", transactionNum);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Withdrawl");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", 0);
            cmdTransaction.Parameters.AddWithValue("@amount", amountTrans);

            con.Open();
            cmdFrom.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();

            return "Withdrawl Successful";
        }
        //Customer Deposit
        public string preformDeposit(int accNumber, float tAmount)
        {
            SqlCommand cmdTo = new SqlCommand("update accounts set accBal = accBal + @amount where accNum = @ToAccount", con);
            cmdTo.Parameters.AddWithValue("@amount", tAmount);
            cmdTo.Parameters.AddWithValue("@ToAccount", accNumber);

            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", 0);
            cmdTransaction.Parameters.AddWithValue("@transNum", 0);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Transfer");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", accNumber);
            cmdTransaction.Parameters.AddWithValue("@amount", tAmount);

            con.Open();
            cmdTo.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();

            return "Deposit Complete";
        }

        //Admin Deposit
        public string preformDeposit(int accNumber, float tAmount, int transNum)
        {
            SqlCommand cmdTo = new SqlCommand("update accounts set accBal = accBal + @amount where accNum = @ToAccount", con);
            cmdTo.Parameters.AddWithValue("@amount", tAmount);
            cmdTo.Parameters.AddWithValue("@ToAccount", accNumber);

            SqlCommand cmdTransaction = new SqlCommand("insert into transactions values(@fromAccount,@transNum,@tranType,@amount,GETDATE(),@ToAccount)", con);
            cmdTransaction.Parameters.AddWithValue("@fromAccount", 0);
            cmdTransaction.Parameters.AddWithValue("@transNum", transNum);
            cmdTransaction.Parameters.AddWithValue("@tranType", "Transfer");
            cmdTransaction.Parameters.AddWithValue("@ToAccount", accNumber);
            cmdTransaction.Parameters.AddWithValue("@amount", tAmount);

            con.Open();
            cmdTo.ExecuteNonQuery();
            cmdTransaction.ExecuteNonQuery();
            con.Close();

            return "Deposit Complete";
        }
    }
}
