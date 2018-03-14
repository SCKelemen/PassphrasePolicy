using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FunWithOnBasePasswordPolicies
{
    public class Passwordpolicy
    {

        /*
        The master pass or fail variable decided based on whether or not the password meets all of the requirements set forth in the password policy:
        FALSE - This variable will be set to false if the currently selected password has failed ANY of the requirements
        TRUE - The variable will be set to TRUE if the currently selected password has passed ALL of the requirements
        */
        bool currentlySelectedPasswordPassOrFail;

        

        //Variables related to password policies:

        #region complexityVariables
        //Variables for Complexity requirements
        bool requiresAlphanumCharsOnly;
        List<string> passedAlphaNumCharsOnlyTest = new List<string>(); //List of passwords that passed the test for alphanum chars only
        List<string> failedAlphaNumCharsOnlyTest = new List<string>(); //List of passwords that failed the test for alphanum chars only

        #region disallowEmbedUserName
        /*
        TODO Enhance this application to allow the use of dictionaries (username:password) to check for embedded username in credential pairs
        //Variables for use in the future when a dictionary of usernames:passwords can be implemented
        bool disallowEmbedUserName;
        */
        #endregion

        bool requiresMaxRepeatedConsChars;
        int maxRepeatedConsChars; //Maximum number of allowed consecutive Characters
        List<string> passedMaxRepeatedConsCharsTest = new List<string>(); //List of passwords that passed the test for maximum number of allowed consecutive characters
        List<string> failedMaxRepeatedConsCharsTest = new List<string>(); //List of passwords that failed the test for maximum number of allowed consecutive characters

        bool requiresCommonSubstringMaxLength;
        int commonSubstringMaxLength; //Common Substring Maximum Length
        List<string> passedCommonSubStringMaxLengthTest = new List<string>(); //List of passwords that passed the test for maximum allowed common substring
        List<string> failedCommonSubStringMaxLenghTest = new List<string>(); //List of passwords that failed the test for maximum allowed common substring

        bool requiresMaxOverallLength;
        int maxOverallLength; //Maximum overall length
        List<string> passedMaxOverallLengthTest = new List<string>(); //List of passwords that passed the test for maximum overall length
        List<string> failedMaxOverallLengthTest = new List<string>(); //List of passwords that failed the test for maximum overall length

        public bool requiresMinOverallLength;
        private int minOverallLength; //Minimum overall length
        private List<string> passedMinOverallLengthTest = new List<string>(); //List of passwords that passed the test for minimum overall length
        private List<string> failedMinOverallLengthTest = new List<string>(); //List of passwords that failed the test for minimum overall length

        public int MinOverallLength { get => minOverallLength; set => minOverallLength = value; } //Automatically applied encapsulation...Not really sure wtf this is.
        public List<string> FailedMinOverallLengthTest { get => failedMinOverallLengthTest; set => failedMinOverallLengthTest = value; } //Automatically applied encapsulation...Not really sure wtf this is.
        public List<string> PassedMinOverallLengthTest { get => passedMinOverallLengthTest; set => passedMinOverallLengthTest = value; } //Automatically applied encapsulation...Not really sure wtf this is.
        #endregion

        #region contentQuotaVariables
        //Variables for Content Quota requirements
        bool requiresAlphaChars; //Requires at least a specific number of alphabetic characters
        int requiredAmtAlphaChars; //Required amount of Alphabetic characters
        List<string> passedRequiresAlphaCharsTest = new List<string>(); //List of passwords that passed the test for required amount of alphabetic characters
        List<string> failedRequiresAlphaCharTest = new List<string>(); //List of passwords that failed the test for required amount of alphabetic characters


        bool requiresNumericChars; //Requires at least a a specific number of numeric characters
        int requiredAmtNumericChars; //Required amount of numeric characters
        List<string> passedRequiresNumericCharsTest = new List<string>(); //List of passwords that passed the test for required amount of numeric characters
        List<string> failedRequiresNumericCharsTest = new List<string>(); //List of passwords that failed the test for required amount of numeric characters

        bool requiresUpperCaseChars; //Requires at least a specific number of uppercase characters
        int requiredAmtUpperCaseChars; //Required amount of uppercase characters
        List<string> passedRequiresUpperCaseCharsTest = new List<string>(); //List of passwords that passed the test for required amount of uppercase characters.
        List<string> failedRequiresUpperCaseCharsTest = new List<string>(); //List of passwords that failed the test for required amount of uppercase characters

        bool requiresLowerCaseChars; //Requires at least a specific number of lowercase characters
        int requiredAmtLowerCaseChars; //Required amount of lowercase characters
        List<string> passedRequiresLowerCaseCharsTest = new List<string>(); //List of passwords that passed the test for required amount of lowercase characters.
        List<string> failedRequiresLowerCaseCharsTest = new List<string>(); //List of passwords that failed the test for required amount of lowercase characters.

        bool requiresSatisfyAtLeastXRules; //Corresponds to the "Satisfy at least <n> Quota Rules check box in Config
        int requiredAmtQuotaRules; //Required amount of content quota rules that will need to be satisfied.
        List<string> passedRequiresSatisfyAtLeastXRulesTest = new List<string>(); //List of passwords that passed the test for the required amount of rules satisfied
        List<string> failedRequiresSatisfyAtLeastXRulesTest = new List<string>(); //List of passwords failed the test for the required amount of rules satisfied

        
        #endregion

        public Passwordpolicy()
        {
            //parameterless constructor to create PasswordPolicy
        }

        #region checkBoxLogic
        //logic to correspond with the requirements checkboxes in the password policies dialog in the configuration module

        public void RequireAlphanumChars(bool requiredTrueOrFalse)
        {
            if (requiredTrueOrFalse == false)
            {
                this.requiresAlphanumCharsOnly = false;

            }
            else if (requiredTrueOrFalse == true)
            {
                this.requiresAlphanumCharsOnly = true;
            }
            else
            {
                //Throw an exception
                //TODO There is probably a better way to handle this...maybe using a raise statement or something else I haven't learned about yet.
                Console.WriteLine("Error!");
                Console.ReadLine();
            }
        }

        public void DisallowEmbeddedUserName()
        {
            //Does nothing for now.
            //TODO make this work when provided with a dictionary containing credential pairs (username:password)
        }

        public void RequireMaxRepeatedConsChars(bool requiredTrueOrFalse, int maxChars)
        {
            if (requiredTrueOrFalse == false)
            {
                this.requiresMaxRepeatedConsChars = false;
            }
            else if (requiredTrueOrFalse == true)
            {
                this.requiresMaxRepeatedConsChars = true;
                this.maxRepeatedConsChars = maxChars;
            }
            else
            {
                //Throw an exception
                //TODO There is probably a better way to handle this...maybe using a raise statement or something else I haven't learned about yet.
                Console.WriteLine("Error!");
                Console.ReadLine();
            }
        }

        public void RequireSubstringMaxLength(bool requiredTrueOrFalse, int maxChars) //See p608 of the SysAdmin17 MRG for details about this requirement.
        {
            if (requiredTrueOrFalse == false)
            {
                this.requiresCommonSubstringMaxLength = false;
            }
            else if (requiredTrueOrFalse == true)
            {
                this.requiresCommonSubstringMaxLength = true;
                this.commonSubstringMaxLength = maxChars;
            }
            else
            {
                //Throw an exception
                //TODO There is probably a better way to handle this...maybe using a raise statement or something else I haven't learned about yet.
                Console.WriteLine("Error!");
                Console.ReadLine();
            }
        }

        public void RequireMaxOverallLength(bool requiredTrueOrFalse, int maxChars)
        {
            if (requiredTrueOrFalse == false)
            {
                this.requiresMaxOverallLength = false;
            }
            else if (requiredTrueOrFalse == true)
            {
                this.requiresMaxOverallLength = true;
                this.maxOverallLength = maxChars;
            }
            else
            {
                //Throw an exception
                //TODO There is probably a better way to handle this...maybe using a raise statement or something else I haven't learned about yet.
                Console.WriteLine("Error!");
                Console.ReadLine();
            }
        }

        public void RequireMinOverallLength(bool requiredTrueOrFalse, int minChars)
        {
            if (requiredTrueOrFalse == false)
            {
                this.requiresMinOverallLength = false;
            }
            else if (requiredTrueOrFalse == true)
            {
                this.requiresMinOverallLength = true;
                this.MinOverallLength = minChars;
            }
        }
        #endregion

        public bool CheckPasswordLength(string password)
        {
            if (password.Length < this.MinOverallLength)
            {
                //Console.WriteLine("The password \"{0}\" is too short, it is {1} characters less than the required length of {2} characters.", password, (this.MinOverallLength - password.Length), this.MinOverallLength);
                FailedMinOverallLengthTest.Add(password);
                //Console.WriteLine("Added " + password + " to list of passwords that failed the test for minimum overall length.");
                return false;
            }
            else
            {
                //Console.WriteLine("The password \"{0}\" is {1} characters and satisfies the requirement for a minimum password length of {2}", password, password.Length, MinOverallLength);
                PassedMinOverallLengthTest.Add(password);
                //Console.WriteLine("Added " + password + " to list of passwords that passed the test for minimum overall length.");
                return true;
            }
        }
    }
}
