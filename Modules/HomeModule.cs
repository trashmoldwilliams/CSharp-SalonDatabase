using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace HairSalonNS
{
  public class HomeModule : NancyModule
  {

    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/clients"] = _ => {
        List<Client> AllClients = Client.GetAll();
        return View["clients.cshtml", AllClients];
      };
      Get["/stylists"] = _ => {
        List<Stylist> AllStylists = Stylist.GetAll();
        return View["stylists.cshtml", AllStylists];
      };

      //Create a new client
      Get["/clients/new"] = _ => {
        return View["clients_form.cshtml"];
      };

      Post["/clients/new"] = _ => {
        Client newClient = new Client(Request.Form["client-name"]);
        newClient.Save();
        return View["success.cshtml"];
      };

      //Create a new stylist
      Get["/stylists/new"] = _ => {
        return View["stylists_form.cshtml"];
      };

      Post["/stylists/new"] = _ => {
        Stylist newStylist = new Stylist(Request.Form["stylist-name"]);
        newStylist.Save();
        return View["success.cshtml"];
      };

      Get["clients/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Client SelectedClient = Client.Find(parameters.id);
        List<Stylist> ClientStylists = SelectedClient.GetStylists();
        List<Stylist> AllStylists = Stylist.GetAll();
        model.Add("client", SelectedClient);
        model.Add("clientStylists", ClientStylists);
        model.Add("allStylists", AllStylists);
        return View["client.cshtml", model];
      };

      Get["stylists/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Stylist SelectedStylist = Stylist.Find(parameters.id);
        List<Client> StylistClients = SelectedStylist.GetClients();
        List<Client> AllClients = Client.GetAll();
        model.Add("stylist", SelectedStylist);
        model.Add("stylistClients", StylistClients);
        model.Add("allClients", AllClients);
        return View["stylist.cshtml", model];
      };

      Post["client/add_stylist"] = _ => {
        Stylist stylist = Stylist.Find(Request.Form["stylist-id"]);
        Client client = Client.Find(Request.Form["client-id"]);
        client.AddStylist(stylist);
        return View["success.cshtml"];
      };

      Post["stylist/add_client"] = _ => {
        Stylist stylist = Stylist.Find(Request.Form["stylist-id"]);
        Client client = Client.Find(Request.Form["client-id"]);
        stylist.AddClient(client);
        return View["success.cshtml"];
      };
    }
  }
}
