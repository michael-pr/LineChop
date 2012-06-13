using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using owaitlist.Models;
using System.Web.Mvc;

namespace owaitlist
{
	[ServiceContract(Namespace="")]
	[SilverlightFaultBehavior]
	[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
	public class Waitlist
	{
        OwlEntities db = new OwlEntities();

        public Waitlist()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
        }

        [OperationContract]
        public Restaurant Check(int id)
        {
            try
            {
                var restaurant = db.Restaurants.Find(id);
                if (restaurant != null)
                {
                    restaurant.Reservations = db.Reservations.Where(r => r.RestaurantId == restaurant.Id).ToList();
                    return restaurant;
                }
                else
                    return null;
            }
            catch (Exception err)
            {
                throw new FaultException("Error accessing the database");
            }
        }

        [Authorize]
        [OperationContract]
        public Restaurant Reserve(Reservation reservation)
        {
            try
            {
                var restaurant = db.Restaurants.Where(r => r.Id == reservation.RestaurantId).SingleOrDefault();
                if (restaurant != null)
                {
                    db.Reservations.Add(reservation);
                    if (restaurant.AutoIncrement)
                        restaurant.WaitTime = restaurant.WaitTime.Add(new TimeSpan((restaurant.Increment.Ticks * reservation.Guests)));
                    db.SaveChanges();
                    restaurant.Reservations = db.Reservations.Where(r => r.RestaurantId == restaurant.Id).ToList();
                    return restaurant;
                }
                else
                    throw new FaultException("Restaurant doesn't exist");
            }
            catch (Exception err)
            {
                throw new FaultException("Error accessing the database");
            }
        }

        [OperationContract]
        public List<Restaurant> Search(string query)
        {
            try
            {
                var restaurants = (from r in db.Restaurants
                                   where r.Name.ToLower().Contains(query.ToLower()) ||
                                   r.Location.ToLower().Contains(query.ToLower())
                                   select r).ToList();
                foreach (var restaurant in restaurants)
                    restaurant.Reservations = db.Reservations.Where(r => r.RestaurantId == restaurant.Id).ToList();
                return restaurants.ToList();
            }
            catch (Exception err)
            {
                throw new FaultException("Error accessing the database");
            }
        }

        [OperationContract]
        public Restaurant GetRestaurant(int id)
        {
            try
            {
                var restaurant = db.Restaurants.Find(id);
                if (restaurant == null)
                {
                    restaurant.Reservations = db.Reservations.Where(r => r.RestaurantId == restaurant.Id).ToList();
                    return restaurant;
                }
                else
                    throw new FaultException("Restaurant doesn't exist");
            }
            catch (Exception err)
            {
                throw new FaultException("Error accessing the database");
            }
        }

        [OperationContract]
        public List<Restaurant> UpdateRestaurants(List<int> restaurantIds)
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            foreach (int id in restaurantIds)
            {
                var restaurant = db.Restaurants.Find(id);
                if (restaurant != null)
                {
                    restaurant.Reservations = db.Reservations.Where(r => r.RestaurantId == restaurant.Id).ToList();
                    restaurants.Add(restaurant);
                }
            }
            return restaurants;
        }
	}
}
