using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

namespace Poker
{
    class Stats
    {
        private string encryptedKey = "cyclotron1984";
        public int Cash { get; set; }
        public int WalletCount { get; set; }
        public int BonusStack { get; set; }
        public int CSpeed { get; set; }
        public int CBackChoice { get; set; }
        public int BonusStats { get; set; }
        public int TwoPairStats { get; set; }
        public int ThreeOfAKindStats { get; set; }
        public int StraightStats { get; set; }
        public int FlushStats { get; set; }
        public int FullHouseStats { get; set; }
        public int FourOfAKindStats { get; set; }
        public int StraightFlushStats { get; set; }
        public int RoyalFlushStats { get; set; }
        public int FiveOfAKindStats { get; set; }
        public int JackpotCounter { get; set; }
        string filePath = @"Resources\Statistics.txt";
        List<string> StatContents;

        //Writes to Statitstics.txt Add more Properties into list for stats, 'c' = CASH, 'w' = WALLET add more parameters to insert more statistics to the list
        public void WriteToFile(int c, int w, int s, int cSpeed, int cBackChoice,
            int bonusStats, int twoPairStats, int threeOfAKindStats, int straightStats,
            int flushStats, int fullHouseStats, int fourOfAKindStats, int straightFlushStats, int royalFlushStats, int fiveOfAKindStats, int  jackpotCounter)
        {
            if (StatContents != null)
            {
                StatContents.Clear();
                StatContents.Add(Encrypt(c.ToString()));
                StatContents.Add(Encrypt(w.ToString()));
                StatContents.Add(Encrypt(s.ToString()));
                StatContents.Add(Encrypt(cSpeed.ToString()));
                StatContents.Add(Encrypt(cBackChoice.ToString()));
                StatContents.Add(Encrypt(bonusStats.ToString()));
                StatContents.Add(Encrypt(twoPairStats.ToString()));
                StatContents.Add(Encrypt(threeOfAKindStats.ToString()));
                StatContents.Add(Encrypt(straightStats.ToString()));
                StatContents.Add(Encrypt(flushStats.ToString()));
                StatContents.Add(Encrypt(fullHouseStats.ToString()));
                StatContents.Add(Encrypt(fourOfAKindStats.ToString()));
                StatContents.Add(Encrypt(straightFlushStats.ToString()));
                StatContents.Add(Encrypt(royalFlushStats.ToString()));
                StatContents.Add(Encrypt(fiveOfAKindStats.ToString()));
                StatContents.Add(Encrypt(jackpotCounter.ToString()));
            }
            else
            {
                StatContents.Add(Encrypt(c.ToString()));
                StatContents.Add(Encrypt(w.ToString()));
                StatContents.Add(Encrypt(s.ToString()));
                StatContents.Add(Encrypt(cSpeed.ToString()));
                StatContents.Add(Encrypt(cBackChoice.ToString()));
                StatContents.Add(Encrypt(bonusStats.ToString()));
                StatContents.Add(Encrypt(twoPairStats.ToString()));
                StatContents.Add(Encrypt(threeOfAKindStats.ToString()));
                StatContents.Add(Encrypt(straightStats.ToString()));
                StatContents.Add(Encrypt(flushStats.ToString()));
                StatContents.Add(Encrypt(fullHouseStats.ToString()));
                StatContents.Add(Encrypt(fourOfAKindStats.ToString()));
                StatContents.Add(Encrypt(straightFlushStats.ToString()));
                StatContents.Add(Encrypt(royalFlushStats.ToString()));
                StatContents.Add(Encrypt(fiveOfAKindStats.ToString()));
                StatContents.Add(Encrypt(jackpotCounter.ToString()));
            }
            
            //Adds the contents of the Statistics text file to a list
            File.WriteAllLines(filePath, StatContents);
            //Writes the ReadMe text file
            File.WriteAllText(@"Resources\ReadMe.txt", " The file Statistics.txt contains encrypted statistics data. If you delete any of this data, you will lose any progress in the game(cash, money inside your wallet etc ).");
           
        }
        //Reads the statistics.txt file
        public void ReadFromFile()
        {
            if (File.Exists(filePath))
            {
                StatContents = File.ReadAllLines(@"Resources\Statistics.txt").ToList();
                if (!StatContents.Any())
                {
                    Cash = 1000;
                    WalletCount = 100;
                    BonusStack = 0;
                    CSpeed = 2;
                    CBackChoice = 2;
                    BonusStack = 0;
                    CSpeed = 2;
                    CBackChoice = 2;
                    BonusStats = 0;
                    TwoPairStats = 0;
                    ThreeOfAKindStats = 0;
                    StraightStats = 0;
                    FlushStats = 0;
                    FullHouseStats = 0;
                    FourOfAKindStats = 0;
                    StraightFlushStats = 0;
                    RoyalFlushStats = 0;
                    FiveOfAKindStats = 0;
                    JackpotCounter = 0;
                    return;
                }
                else
                {
                    Cash = Convert.ToInt32(Decrypt(StatContents[0].ToString()));
                    WalletCount = Convert.ToInt32(Decrypt(StatContents[1].ToString()));
                    BonusStack = Convert.ToInt32(Decrypt(StatContents[2].ToString()));
                    CSpeed = Convert.ToInt32(Decrypt(StatContents[3].ToString()));
                    CBackChoice = Convert.ToInt32(Decrypt(StatContents[4].ToString()));
                    BonusStats = Convert.ToInt32(Decrypt(StatContents[5].ToString()));
                    TwoPairStats = Convert.ToInt32(Decrypt(StatContents[6].ToString()));
                    ThreeOfAKindStats = Convert.ToInt32(Decrypt(StatContents[7].ToString()));
                    StraightStats = Convert.ToInt32(Decrypt(StatContents[8].ToString()));
                    FlushStats = Convert.ToInt32(Decrypt(StatContents[9].ToString()));
                    FullHouseStats = Convert.ToInt32(Decrypt(StatContents[10].ToString()));
                    FourOfAKindStats = Convert.ToInt32(Decrypt(StatContents[11].ToString()));
                    StraightFlushStats = Convert.ToInt32(Decrypt(StatContents[12].ToString()));
                    RoyalFlushStats = Convert.ToInt32(Decrypt(StatContents[13].ToString()));
                    FiveOfAKindStats = Convert.ToInt32(Decrypt(StatContents[14].ToString()));
                    JackpotCounter = Convert.ToInt32(Decrypt(StatContents[15].ToString()));
                    return;
                }
            }
            else
            {
                File.Create(filePath);
                Cash = 1000;
                WalletCount = 100;
                BonusStack = 0;
                CSpeed = 2;
                CBackChoice = 2;
                BonusStats = 0;
                TwoPairStats = 0;
                ThreeOfAKindStats = 0;
                StraightStats = 0;
                FlushStats = 0;
                FullHouseStats = 0;
                FourOfAKindStats = 0;
                StraightFlushStats = 0;
                RoyalFlushStats = 0;
                FiveOfAKindStats = 0;
                JackpotCounter = 0;
                return;
            }
            
        }
        //Code for Encryption:
        private static string Encrypt(string clearText)
        {
            Stats stats = new Stats();
            string EncryptionKey = stats.encryptedKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        // Code for Decryption: 
        private static string Decrypt(string cipherText)
        {
            Stats stats = new Stats();
            string EncryptionKey = stats.encryptedKey;
            cipherText = cipherText.Replace(" ", "+");
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There Was An Encryption Error \n All Saved Data Has Been Lost or Deleted by the User \n Restart Red Black Poker To Reset All Data");
                stats.encryptedKey = "cyclotron1984";
                stats.Cash = 1000;
                stats.WalletCount = 100;
                stats.BonusStack = 0;
            }
            return cipherText;
        }
    }
}
