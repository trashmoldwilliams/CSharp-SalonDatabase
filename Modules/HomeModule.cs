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

      Get["/stylists"] = _ => {
        List<Stylist> AllStylists = Stylist.GetAll();
        return View["stylists.cshtml", AllStylists];
      };

      Post["/stylists/new"] = _ => {
        Stylist newStylist = new Stylist(Request.Form["stylist-name"]);
        newStylist.Save();
        List<Stylist> AllStylists = Stylist.GetAll();
        return View["stylists.cshtml", AllStylists];
      };

      Get["/stylists/delete_all"] = _ => {
        Stylist.DeleteAll();
        List<Stylist> AllStylists = Stylist.GetAll();
        return View["stylists.cshtml", AllStylists];
      };

      Get["/stylists/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedStylist = Stylist.Find(parameters.id);
        var StylistClient = SelectedStylist.GetClients();
        List<Stylist> AllStylists = Stylist.GetAll();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClient);
        model.Add("stylists", AllStylists);
        return View["clients.cshtml", model];
      };

      Post["/stylists/{id}/new"] = parameters => {
        Client newClient = new Client(Request.Form["name"], Request.Form["stylist-id"]);
        newClient.Save();

        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedStylist = Stylist.Find(parameters.id);
        var StylistClient = SelectedStylist.GetClients();
        List<Stylist> AllStylists = Stylist.GetAll();
        model.Add("stylist", SelectedStylist);
        model.Add("clients", StylistClient);
        model.Add("stylists", AllStylists);
        return View["clients.cshtml", model];
      };
    }
  }
}
