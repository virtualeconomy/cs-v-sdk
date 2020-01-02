# cs-v-sdk
C# library for V Systems

## Install

  1. To use this SDK, we need .Net framework 4.5.1 or above. If your OS does not have this framework, please download it [here](https://docs.microsoft.com/en-us/dotnet/framework/install/guide-for-developers).
  
  2. clone this project

     ```bash
     $ git clone https://github.com/virtualeconomy/cs-v-sdk.git
     ```
	 
  3. The external libraries using are under `libs` folder. Here is source code info:

    * `Blake2Sharp.dll` is from https://github.com/BLAKE2/BLAKE2/tree/master/csharp
	 
    * `HashLib.dll` is from https://github.com/stratisproject/HashLib

## Usage

### Create chain object
1. For testnet chain:

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
    
    Blockchain chain = new Blockchain(NetworkType.Testnet, "http://test.v.systems:9922");
    ```

2. For mainnet chain:

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
 
    Blockchain chain = new Blockchain(NetworkType.Mainnet, "https://wallet.v.systems/api");
    ```
    
### Create address object
1. Create account by seed

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
 
    Account acc = new Account(NetworkType.Testnet, "<your seed>", 0);
    ```

2. Create account by private key

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
     
    Account acc = new Account(NetworkType.Testnet, "<base58 private key>");
    ```
 
3. Create account by public key

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
     
    Account acc = new Account(NetworkType.Testnet, "<base58 public key>", null);
    ```
    
4. Create account by address

    ```csharp
    using v.systems;
    using v.systems.type;
    
    ...
     
    Account acc = new Account(NetworkType.Testnet, null, "<base58 address>");
    ```
    
### Send transaction
1. Send Payment transaction

    ```csharp
    long amount = 1 * Blockchain.V_UNITY;  // Send 1.0 V coin
    PaymentTransaction tx = TransactionFactory.BuildPaymentTx("<recipient address>", amount);
    
    // Usage 1: for hot wallet sending payment transaction
    ITransaction result = acc.SendTransaction(chain, tx);
    
    // Usage 2: for cold wallet signing payment transaction
    string signature = acc.GetSignature(tx);
    ```

2. Send Lease transaction

    ```csharp
    long amount = 1 * Blockchain.V_UNITY;  // Lease 1.0 V coin
    LeaseTransaction tx = TransactionFactory.BuildLeaseTx("<recipient address>", amount);
    
    // Usage 1: for hot wallet sending lease transaction
    ITransaction result = acc.SendTransaction(chain, tx);
    
    // Usage 2: for cold wallet signing lease transaction
    string signature = acc.GetSignature(tx);
    ```
