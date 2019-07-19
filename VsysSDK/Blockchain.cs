using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using v.systems.entity;
using v.systems.error;
using v.systems.transaction;
using v.systems.type;
using v.systems.utils;

namespace v.systems
{
    public class Blockchain
    {
        public const long V_UNITY = 100000000L;
        public const int DEFAULT_TX_LIMIT = 100;

        public Blockchain(NetworkType network, string nodeUrl)
        {
            this.Network = network;
            this.NodeUrl = nodeUrl;
        }

        public long? GetBalance(string address)
        {
            string url = string.Format("{0}/addresses/balance/{1}", NodeUrl, address);
            BalanceData balance = this.CallChainAPI<BalanceData>(url);
            return balance.Balance;
        }

        public BalanceDetail GetBalanceDetail(string address)
        {
            string url = string.Format("{0}/addresses/balance/details/{1}", NodeUrl, address);
            return this.CallChainAPI<BalanceDetail>(url);
        }

        public IList<ITransaction> GetTransactionHistory(string address)
        {
            return GetTransactionHistory(address, DEFAULT_TX_LIMIT);
        }

        public IList<ITransaction> GetTransactionHistory(string address, int num)
        {
            IList<ITransaction> result = new List<ITransaction>();
            if (num <= 0)
            {
                return result;
            }
            string url = string.Format("{0}/transactions/address/{1}/limit/{2}", NodeUrl, address, num);
            string json = HttpHelper.Get(url);
            try
            {
                List<List<object>> list = JsonConvert.DeserializeObject<List<List<object>>>(json);
                if (list.Count == 0)
                {
                    return result;
                }
                List<object> txList = list[0];
                foreach (object txObj in txList)
                {
                    string txStr = JsonConvert.SerializeObject(txObj);
                    ITransaction tx = TransactionParser.Parse(txStr);
                    result.Add(tx);
                }
            }
            catch (Exception)
            {
                throw ApiError.FromJson(json);
            }
            return result;
        }

        public ITransaction GetTransactionById(string txId)
        {
            string url = string.Format("{0}/transactions/info/{1}", NodeUrl, txId);
            string json = HttpHelper.Get(url);
            ITransaction tx;
            try
            {
                tx = TransactionParser.Parse(json);

            }
            catch (Exception)
            {
                throw TransactionError.FromJson(json);
            }
            if (tx != null && tx.Status.Equals("error"))
            {
                throw TransactionError.FromJson(json);
            }
            return tx;
        }

        public ITransaction GetUnconfirmedTransactionById(string txId)
        {
            string url = string.Format("{0}/transactions/unconfirmed/info/{1}", NodeUrl, txId);
            string json = HttpHelper.Get(url);
            try
            {
                return TransactionParser.Parse(json);
            }
            catch (Exception)
            {
                throw TransactionError.FromJson(json);
            }
        }

        public ProvenTransaction SendTransaction(TransactionType txType, string json)
        {
            string url;
            switch (txType)
            {
                case TransactionType.Payment:
                    url = string.Format("{0}/vsys/broadcast/payment", NodeUrl);
                    return this.CallChainAPI<PaymentTransaction>(url, json);
                case TransactionType.Lease:
                    url = string.Format("{0}/leasing/broadcast/lease", NodeUrl);
                    return this.CallChainAPI<LeaseTransaction>(url, json);
                case TransactionType.LeaseCancel:
                    url = string.Format("{0}/leasing/broadcast/cancel", NodeUrl);
                    return this.CallChainAPI<LeaseCancelTransaction>(url, json);
                default:
                    throw new ApiError("Unsupported Transaction Type");
            }
        }

        public int GetHeight()
        {
            string url = string.Format("{0}/blocks/height", NodeUrl);
            string json = HttpHelper.Get(url);
            try
            {
                var definition = new { Height = 0 };
                var result = JsonConvert.DeserializeAnonymousType(json, definition);
                return result.Height;
            }
            catch (Exception)
            {
                throw ApiError.FromJson(json);
            }
        }

        public Block GetLastBlock()
        {
            string url = String.Format("%s/blocks/last", NodeUrl);
            return this.CallChainAPI<Block>(url);
        }
        public Block GetBlockByHeight(int height)
        {
            string url = String.Format("%s/blocks/at/%d", NodeUrl, height);
            return this.CallChainAPI<Block>(url);
        }

        // TODO: implement these functions later
        // getTokenInfo(String tokenId)
        // getTokenBalance(String address, String tokenId)
        // getContractInfo(String contractId)
        // getContractContent(String contractId)

        private T CallChainAPI<T>(string url)
        {
            string json = HttpHelper.Get(url);
            return ParseResponse<T>(json);
        }

        private T CallChainAPI<T>(string url, string jsonData)
        {
            string json = HttpHelper.Post(url, jsonData);
            return ParseResponse<T>(json);
        }

        private T ParseResponse<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw ApiError.FromJson(json);
            }
        }

        public NetworkType Network { get; }

        public string NodeUrl { get; }
    }
}