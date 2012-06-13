using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using owaitlist.Models;

namespace owaitlist
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWaitlist" in both code and config file together.
    [ServiceContract]
    public interface IWaitlist
    {
        [OperationContract]
        Restaurant Check(int id);

        [OperationContract]
        Restaurant Reserve(Reservation reservation);

        [OperationContract]
        List<Restaurant> Search(string query);

        [OperationContract]
        Restaurant GetRestaurant(int id);

        [OperationContract]
        List<Restaurant> UpdateRestaurants(List<int> restaurantIds);
    }
}
