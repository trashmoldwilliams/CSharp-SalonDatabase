using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalonNS
{
  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_ClientsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Client.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSame()
    {
      //Arrange, Act
      Client firstClient = new Client("Jerry", 1);
      Client secondClient = new Client("Jerry", 1);

      //Assert
      Assert.Equal(firstClient, secondClient);
    }

    [Fact]
    public void Test_Save_SavesClientToDatabase()
    {
      //Arrange
      Client testClient = new Client("Jerry", 1);
      testClient.Save();

      //Act
      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToClientObject()
    {
      //Arrange
      Client testClient = new Client("Jerry", 1);
      testClient.Save();

      //Act
      Client savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsClientInDatabase()
    {
      //Arrange
      Client testClient = new Client("Jerry", 1);
      testClient.Save();

      //Act
      Client foundClient = Client.Find(testClient.GetId());

      //Assert
      Assert.Equal(testClient, foundClient);
    }

    [Fact]
    public void Test_Delete_DeletesClientFromDatabase()
    {
      //Arrange
      string name1 = "Larry";
      Stylist testStylist1 = new Stylist(name1);
      testStylist1.Save();

      Client testClient1 = new Client("Jerry", testStylist1.GetId());
      testClient1.Save();
      Client testClient2 = new Client("Perry", testStylist1.GetId());
      testClient2.Save();

      //Act
      testClient1.Delete();
      List<Client> resultClients = testStylist1.GetClients();
      List<Client> testClientList = new List<Client> {testClient2};

      //Assert
      Assert.Equal(testClientList, resultClients);
    }

    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }

  }
}
