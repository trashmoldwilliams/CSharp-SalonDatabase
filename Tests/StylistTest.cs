using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace HairSalonNS
{
  public class StylistTest : IDisposable
  {
    public StylistTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_StylistsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Stylist.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSame()
    {
      //Arrange, Act
      Stylist firstStylist = new Stylist("Larry");
      Stylist secondStylist = new Stylist("Larry");

      //Assert
      Assert.Equal(firstStylist, secondStylist);
    }

    [Fact]
    public void Test_Save_SavesStylistToDatabase()
    {
      //Arrange
      Stylist testStylist = new Stylist("Larry");
      testStylist.Save();

      //Act
      List<Stylist> result = Stylist.GetAll();
      List<Stylist> testList = new List<Stylist>{testStylist};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToStylistObject()
    {
      //Arrange
      Stylist testStylist = new Stylist("Larry");
      testStylist.Save();

      //Act
      Stylist savedStylist = Stylist.GetAll()[0];

      int result = savedStylist.GetId();
      int testId = testStylist.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsStylistInDatabase()
    {
      //Arrange
      Stylist testStylist = new Stylist("Larry");
      testStylist.Save();

      //Act
      Stylist foundStylist = Stylist.Find(testStylist.GetId());

      //Assert
      Assert.Equal(testStylist, foundStylist);
    }

    [Fact]
    public void Test_GetClients_RetrievesAllClientsWithStylist()
    {
      Stylist testStylist = new Stylist("Larry");
      testStylist.Save();

      Client firstClient = new Client("Jerry", testStylist.GetId());
      firstClient.Save();
      Client secondClient = new Client("Jerry", testStylist.GetId());
      secondClient.Save();


      List<Client> testClientList = new List<Client> {firstClient, secondClient};
      List<Client> resultClientList = testStylist.GetClients();

      Assert.Equal(testClientList, resultClientList);
    }

    [Fact]
    public void Test_Update_UpdatesStylistInDatabase()
    {
      //Arrange
      string name = "Larry";
      Stylist testStylist = new Stylist(name);
      testStylist.Save();
      string newName = "Barry";

      //Act
      testStylist.Update(newName);

      string result = testStylist.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_Delete_DeletesStylistFromDatabase()
    {
      //Arrange
      string name1 = "Larry";
      Stylist testStylist1 = new Stylist(name1);
      testStylist1.Save();

      string name2 = "Barry";
      Stylist testStylist2 = new Stylist(name2);
      testStylist2.Save();

      Client testClient1 = new Client("Jerry", testStylist1.GetId());
      testClient1.Save();
      Client testClient2 = new Client("Perry", testStylist2.GetId());
      testClient2.Save();

      //Act
      testStylist1.Delete();
      List<Stylist> resultStylists = Stylist.GetAll();
      List<Stylist> testStylistList = new List<Stylist> {testStylist2};

      List<Client> resultClients = Client.GetAll();
      List<Client> testClientList = new List<Client> {testClient2};

      //Assert
      Assert.Equal(testStylistList, resultStylists);
      Assert.Equal(testClientList, resultClients);
    }

    public void Dispose()
    {
      Client.DeleteAll();
      Stylist.DeleteAll();
    }
  }
}
