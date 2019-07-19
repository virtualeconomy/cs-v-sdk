using System;
using System.Collections.Generic;
using System.Text;
using org.whispersystems.curve25519.csharp;
using org.whispersystems.curve25519;
using v.systems.type;
using v.systems.serialization;
using v.systems.transaction;
using v.systems.utils;
using v.systems.entity;
using v.systems.error;
using System.IO;
using Newtonsoft.Json.Linq;

namespace v.systems
{
    public class Account
    {
        private static readonly Curve25519 cipher = Curve25519.getInstance(Curve25519.BEST);
        private const byte ADDR_VERSION = 5;

        private readonly byte[] privateKey;
        private readonly byte[] publicKey;
        private byte[] address;
        private NetworkType network;

        public Account(NetworkType network, string seed, int? nonce)
        {
            this.network = network;
            if (nonce != null)
            {
                seed = nonce.ToString() + seed;
            }
            byte[] seedBytes = Encoding.UTF8.GetBytes(seed);
            byte[] accountSeed = Hash.SecureHash(seedBytes);
            byte[] hashedSeed = Hash.SHA256(accountSeed);
            privateKey = new byte[32];
            Buffer.BlockCopy(hashedSeed, 0, privateKey, 0, 32);
            privateKey[0] &= 248;
            privateKey[31] &= 127;
            privateKey[31] |= 64;
            publicKey = new byte[32];
            Curve_sigs.curve25519_keygen(publicKey, privateKey);
            address = PublicKeyToAddress(publicKey, network);
        }

        public Account(NetworkType network, string base58PrivateKey)
        {
            this.network = network;
            privateKey = Base58.Decode(base58PrivateKey);
            publicKey = new byte[32];
            Curve_sigs.curve25519_keygen(publicKey, privateKey);
            address = PublicKeyToAddress(publicKey, network);
        }

        public Account(NetworkType network, string base58PublicKey, string base58Address)
        {
            this.network = network;
            if (base58PublicKey != null)
            {
                publicKey = Base58.Decode(base58PublicKey);
                address = PublicKeyToAddress(publicKey, network);
            }
            else
            {
                address = Base58.Decode(base58Address);
                if (!CheckAddress())
                {
                    throw new VException("invalid address");
                }
            }
        }

        public ProvenTransaction SendTransaction(Blockchain chain, ProvenTransaction tx)
        {
            TransactionType txType = (TransactionType)tx.Type;
            string signature = GetSignature(tx);
            JObject json = tx.ToAPIRequestJson(this.PublicKey, signature);
            return chain.SendTransaction(txType, json.ToString());
        }

        public string GetSignature(BytesSerializable tx)
        {
            return GetSignature(tx.ToBytes());
        }

        public string GetSignature(byte[] bytes)
        {
            if (privateKey == null)
            {
                throw new KeyError("Cannot sign the context. No private key in account.");
            }
            return Base58.Encode(cipher.calculateSignature(this.privateKey, bytes));
        }

        public long? GetBalance(Blockchain chain)
        {
            return chain.GetBalance(this.Address);
        }

        public BalanceDetail GetBalanceDetail(Blockchain chain)
        {
            return chain.GetBalanceDetail(this.Address);
        }

        public IList<ITransaction> GetTransactionHistory(Blockchain chain, int num)
        {
            return chain.GetTransactionHistory(this.Address, num);
        }

        public string PrivateKey
        {
            get
            {
                if (privateKey == null)
                {
                    throw new KeyError("No private key in account.");
                }
                return Base58.Encode(privateKey);
            }
        }

        public string PublicKey
        {
            get
            {
                if (publicKey == null)
                {
                    throw new KeyError("No public key in account.");
                }
                return Base58.Encode(publicKey);
            }
        }

        public string Address
        {
            get
            {
                if (address == null)
                {
                    if (publicKey == null)
                    {
                        throw new KeyError("No public key in account.");
                    }
                    address = PublicKeyToAddress(publicKey, network);
                }
                return Base58.Encode(address);
            }
        }

        public static string PublicKeyToAddress(string publicKey, NetworkType network)
        {
            return Base58.Encode(PublicKeyToAddress(Base58.Decode(publicKey), network));
        }

        public static byte[] PublicKeyToAddress(byte[] publicKey, NetworkType network)
        {
            var stream = new MemoryStream(26);
            var hash = Hash.SecureHash(publicKey, 0, publicKey.Length);
            var writer = new BinaryWriter(stream);
            writer.Write(ADDR_VERSION);
            writer.Write((byte)network);
            writer.Write(hash, 0, 20);
            var checksum = Hash.SecureHash(stream.ToArray(), 0, 22);
            writer.Write(checksum, 0, 4);
            return stream.ToArray();
        }

        public bool CheckAddress()
        {
            return CheckAddress(this.network, this.address);
        }

        public static bool CheckAddress(NetworkType network, string base58Address)
        {
            try
            {
                return CheckAddress(network, Base58.Decode(base58Address));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckAddress(NetworkType network, byte[] address)
        {
            if (address.Length != 26 || address[0] != ADDR_VERSION || address[1] != (byte)network)
            {
                return false;
            }
            byte[] actualChecksum = Hash.SecureHash(address, 0, 22);
            for (int i = 0; i < 4; i++)
            {
                if (address[i + 22] != actualChecksum[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
