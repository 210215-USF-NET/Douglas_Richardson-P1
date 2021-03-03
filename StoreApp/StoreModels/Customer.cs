using System.ComponentModel.DataAnnotations;
using System;
namespace StoreModels
{
    /// <summary>
    /// Customer model, user is able to put in their name and email address.
    /// </summary>
    public class Customer
    {
        private string firstName;
        private string lastName;
        private string emailAddress;
        
        public int? Id{get;set;}
        public string FirstName { 
            get{return firstName;} 
            set{
                if(value == null || value.Equals("")){
                    throw new Exception("Your first name cannot be empty.");
                }
                firstName = value;
            } 
        }
        public string LastName { 
            get{return lastName;} 
            set{
                if(value == null || value.Equals("")){
                    throw new Exception("Your last name cannot be empty.");
                }
                lastName = value;
            } 
        }
        public string EmailAddress { 
            get{return emailAddress;} 
            set{
                if(value == null || value.Equals("")){
                    throw new Exception("Your email address cannot be empty.");
                }else if(!(new EmailAddressAttribute().IsValid(value))){
                    throw new Exception("Please type in a valid email address.");
                }
                emailAddress = value;
            } 
        }
    }
}