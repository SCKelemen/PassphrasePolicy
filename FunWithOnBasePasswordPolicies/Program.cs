using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FunWithOnBasePasswordPolicies.Rules;

namespace FunWithOnBasePasswordPolicies
{
    class Program
    {
        static void Main(string[] args)
        {
            SWFunction func = new MaximumRepeatedConsecutiveCharactersFunc().Function;
            MaximumRepeatedConsecutiveCharacters mrcc = new MaximumRepeatedConsecutiveCharacters(2, func);
            Console.WriteLine("Google {0}", mrcc.Verify("Google") ? "Valid" : "Not Valid");
            Console.WriteLine("Gooogle {0}", mrcc.Verify("Gooogle") ? "Valid" : "Not Valid");
            Console.WriteLine("Goooogle {0}", mrcc.Verify("Goooogle") ? "Valid" : "Not Valid");

            RuleSet myRuleSet = new RuleSet();
            myRuleSet.Rules.Add(new MinimumLengthRule(15));

            foreach(IRule rule in myRuleSet.Rules)
            {
                rule.Verify(password);
            }

            //Define variables
            int passwordPolicyChoice = 0; //variable for choosing medium or high security password policies, support for custom password policies pending future development

            //Parameters for medium security passwword policy
            int mediumSecurityMaxRepeatedConsChars = 2;
            int mediumSecurityAllowedLength = 8;

            //tally of passwords
            int tallyOfPasswords = 0;

            //Parameters for high security password policy
            int highSecurityMaxRepeatedConsChars = 2;
            int highSecurityAllowedLength = 15;

            //Variables for testing
            bool passedLengthTest;
            bool passedMaxRepeatConsCharsTest;

            //Creates list of passwords
            List<string> passwords = new List<string>();

            //Master list of passwords that have passed ALL tests
            List<string> MasterListAllowedPasswords = new List<string>();
            int numPasswordsPassedALLTests = 0;
            double percentPasswordsPassedALLTests = 0.00f;

            //variables for passwords that fail the length test
            List<string> failedLengthTest = new List<string>();
            int numPasswordsFailedLengthTest = 0;
            int numPasswordsPassedLengthTest = 0;
            double percentPasswordsFailedLengthTest = 0.00f;

            //variables for passwords that fail the max repeated consecutive characters test
            List<string> ListFailedMaxRepeatConsCharTest = new List<string>();
            List<string> ListPassedMaxRepeatConsCharTest = new List<string>();
            int numPasswordsFailedMaxRepeatConsCharTest = 0;
            int numPasswordsPassedMaxRepeatConsCharTest = 0;
            double percentPasswordsFailedMaxRepeatConsCharTest = 0.00f;

            //Create an instance of a password policy object
            Passwordpolicy SelectedPasswordPolicy;
            SelectedPasswordPolicy = new Passwordpolicy();

            #region selectPasswordPolicy
            //Determine the password policy that will be used to check the passwords
            while (passwordPolicyChoice != 1 && passwordPolicyChoice != 2)
            {
                Console.WriteLine("Select an OnBase Password Policy to evaluate passwords with:\n1) Medium Security Password Policy\n2) High Security Password Policy\n(enter 1 or 2)\n>");
                passwordPolicyChoice = Convert.ToInt32(Console.ReadLine());

                if (passwordPolicyChoice == 1) //Medium Security - Default Level Security
                {
                    SelectedPasswordPolicy.RequireAlphanumChars(false);
                    // SelectedPasswordPolicy.DisallowEmbeddedUserName(); //unnecessary at this time
                    SelectedPasswordPolicy.RequireMaxRepeatedConsChars(true, mediumSecurityMaxRepeatedConsChars);
                    SelectedPasswordPolicy.RequireSubstringMaxLength(false, 0);
                    SelectedPasswordPolicy.RequireMaxOverallLength(false, 0);
                    SelectedPasswordPolicy.RequireMinOverallLength(true, mediumSecurityAllowedLength);
                    Console.WriteLine("You've selected the Medium Security Password Policy. Press any key to continue");
                    Console.ReadLine();
                }
                else if (passwordPolicyChoice == 2) //High Security - Recommended Level Security
                {
                    SelectedPasswordPolicy.RequireAlphanumChars(false);
                    // SelectedPasswordPolicy.DisallowEmbeddedUserName(); //unnecessary at this time
                    SelectedPasswordPolicy.RequireMaxRepeatedConsChars(true, 2);
                    SelectedPasswordPolicy.RequireSubstringMaxLength(false, 0);
                    SelectedPasswordPolicy.RequireMaxOverallLength(false, 0);
                    SelectedPasswordPolicy.RequireMinOverallLength(true, highSecurityAllowedLength);
                    //Notify the user that they've selected the high security password policy
                    Console.WriteLine("You've selected the High Security Password Policy. Press any key to continue");
                    Console.ReadLine();
                }
                else
                {
                    //eventually will need to add else if for option to create your own custom password policy
                    Console.WriteLine("Invalid selection. Please enter either 1 or 2!\n");
                }
            }
            #endregion

            #region AddPasswordsToAList
            //Add all passwords in rockyou.txt to a list.

            //opens the file to be read, which contains passwords
            string filepath = "..\\..\\..\\TestData\\PasswordLists\\MasterList.txt";
            StreamReader reader = new StreamReader(filepath);
            string nextLine;
            while ((nextLine = reader.ReadLine()) != null)
            {
                //Console.WriteLine("Adding {0} to the password list", nextLine);
                passwords.Add(nextLine);
            }
            int totalNumPasswords = passwords.Count;
            Console.WriteLine(totalNumPasswords + " passwords have been added to the passwords list.");
            Console.ReadLine();
            #endregion

            //Create report file
            #region CreateReportFile

            string reportDirectory = "C:\\PasswordLists\\Reports";
            Directory.CreateDirectory(reportDirectory); //Creates a directory if one does not exist
            string outputPath = reportDirectory + "\\" + "report.txt";
            StreamWriter writer = new StreamWriter(outputPath, false); //if true, will append text to files that already exist

            /* Commenting out the code below since it broke the program.
            //System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            //System.Windows.Forms.DialogResult Result = ofd.ShowDialog();
            //if (Result == System.Windows.Forms.DialogResult.OK)
            //{
            //    string reportDirectory = "..\\..\\..\\Reports";
            //    Directory.CreateDirectory(reportDirectory); //Creates a directory if one does not exist
            //    string outputPath = reportDirectory + "\\" + "report.txt";
            //    StreamWriter writer = new StreamWriter(ofd.OpenFile()); //if true, will append text to files that already exist
            */


            #endregion

            foreach (var password in passwords)
                {
                    //reset all the test variables
                    string currentlySelectedPassword = Convert.ToString(password);

                    //add one to the tally
                    tallyOfPasswords += 1;

                    #region PasswordCheck
                    //For future use, logic is embedded in this method to create a list of passwords that passed as well as a list of those that failed.
                    passedLengthTest = SelectedPasswordPolicy.CheckPasswordLength(currentlySelectedPassword);
                    if (currentlySelectedPassword.Length >= 2)
                    {
                        passedMaxRepeatConsCharsTest = mrcc.Verify(currentlySelectedPassword);
                    }
                    else
                    {
                        passedMaxRepeatConsCharsTest = false;
                    }

                    if (passedLengthTest == false || passedMaxRepeatConsCharsTest == false) //TODO if ANY tests have failed, do this
                    {
                        //start out by writing to the report file that the password has failed:
                        writer.WriteLine("\nPassword # " + tallyOfPasswords + ": " + "\"" + currentlySelectedPassword + "\"" + "\t\t[FAIL]");

                        //If we're here, it's because either the lengthTest or the MaxConsChars test has failed.
                        //Start out by checking the length test, write a different message to the file dependent on pass or fail.
                        if (passedLengthTest == false)
                        {
                            //Write out why the password failed the length test...
                            writer.WriteLine("[FAILED] The password \"{0}\" is too short of the {1} character requirement by {2} characters", currentlySelectedPassword, SelectedPasswordPolicy.MinOverallLength, (SelectedPasswordPolicy.MinOverallLength - currentlySelectedPassword.Length));
                        }
                        else if (passedLengthTest == true)
                        {
                            writer.WriteLine("[PASSED] The password \"{0}\" meets the password length requirement. (Required: {1} characters, \"{0}\" length: {2} characters)", currentlySelectedPassword, SelectedPasswordPolicy.MinOverallLength, currentlySelectedPassword.Length);
                        }
                        else
                        {
                            //Should never happen. Thrown an error
                            Console.WriteLine("ERROR!");
                            Console.ReadLine();
                        }

                        //Next, check if the Max Repeated Consecutive Chars test is the one that failed
                        if (passedMaxRepeatConsCharsTest == false)
                        {
                            writer.WriteLine("[FAILED] \"{0}\" contains too many repeated consecutive characters!", currentlySelectedPassword);
                            ListFailedMaxRepeatConsCharTest.Add(currentlySelectedPassword);
                            //Uncomment the following line(s) if you wish to debug
                            //Console.WriteLine("Password #{0}: \"{1}\" has failed the test for repeating consecutive characters!", tallyOfPasswords, currentlySelectedPassword);
                            //Console.WriteLine("Current count of passwords that have failed the test for repeating consecutive characters: {0}", ListFailedMaxRepeatConsCharTest.Count);
                        }
                        else if (passedMaxRepeatConsCharsTest == true)
                        {
                            writer.WriteLine("[PASSED] \"{0}\" does not contain too many repeated consecutive characters", currentlySelectedPassword);
                            ListPassedMaxRepeatConsCharTest.Add(currentlySelectedPassword);
                        }
                        else
                        {
                            //This should never happen. Throw an error
                            Console.WriteLine("Error!");
                            Console.ReadLine();
                        }
                    }
                    else if (passedLengthTest == true && passedMaxRepeatConsCharsTest == true) //TODO if ALL tests have passed, do this
                    {
                        //Add the password to the master list of all passwords 
                        MasterListAllowedPasswords.Add(currentlySelectedPassword);

                        writer.WriteLine("\nPassword #" + tallyOfPasswords + ": " + "\"" + currentlySelectedPassword + "\"" + "\t\t[PASS]");

                        //Write out why the password passed the length test...
                        writer.WriteLine("[PASSED] The password \"{0}\" meets the password length requirement. (Required: {1} characters, \"{0}\" length: {2} characters)", currentlySelectedPassword, SelectedPasswordPolicy.MinOverallLength, currentlySelectedPassword.Length);

                        //Write out that the password has passed the repeated cons chars test...
                        writer.WriteLine("[PASSED] \"{0}\" does not contain too many repeated consecutive characters", currentlySelectedPassword);
                        ListPassedMaxRepeatConsCharTest.Add(currentlySelectedPassword);
                    }
                    else
                    {
                        //This should not happen. Throw an exception.
                        Console.WriteLine("Error!");
                    }
                    #endregion

                }


                //Display count
                //TODO The math below doesn't check out. Fix it.
                numPasswordsPassedLengthTest = SelectedPasswordPolicy.PassedMinOverallLengthTest.Count;
                numPasswordsFailedLengthTest = SelectedPasswordPolicy.FailedMinOverallLengthTest.Count;
                percentPasswordsFailedLengthTest = ((Convert.ToDouble(numPasswordsFailedLengthTest)) / (Convert.ToDouble(totalNumPasswords)));
                percentPasswordsFailedLengthTest = percentPasswordsFailedLengthTest * 100;

                //Math regarding max repeating consecutive characters test
                numPasswordsPassedMaxRepeatConsCharTest = ListPassedMaxRepeatConsCharTest.Count;
                numPasswordsFailedMaxRepeatConsCharTest = ListFailedMaxRepeatConsCharTest.Count;
                percentPasswordsFailedMaxRepeatConsCharTest = ((Convert.ToDouble(numPasswordsFailedMaxRepeatConsCharTest)) / (Convert.ToDouble(totalNumPasswords)));
                percentPasswordsFailedMaxRepeatConsCharTest = percentPasswordsFailedMaxRepeatConsCharTest * 100;

                //Math for the master list of allowed passwords that have passed all tests.
                numPasswordsPassedALLTests = MasterListAllowedPasswords.Count;
                percentPasswordsPassedALLTests = ((Convert.ToDouble((numPasswordsPassedALLTests)) / (Convert.ToDouble(totalNumPasswords))));
                percentPasswordsPassedALLTests = percentPasswordsPassedALLTests * 100;

                //Write the report to the report file
                writer.WriteLine("{0} passwords passed the length test", numPasswordsPassedLengthTest);
                writer.WriteLine("{0} out of {1} passwords are insufficient length.", numPasswordsFailedLengthTest, totalNumPasswords);
                writer.WriteLine("A password policy requiring a length of {0} characters would result in {1}% of the passwords in this file being forbidden from use", SelectedPasswordPolicy.MinOverallLength, percentPasswordsFailedLengthTest); //TODO This is not working, figure out how to calculate percentages in C#

                //Write results for max repeating consecutive characters test
                writer.WriteLine("{0} passwords passed the test for maximum repeated consecutive characters", numPasswordsPassedMaxRepeatConsCharTest);
                writer.WriteLine("{0} passwords failed the test for maximum repeated consecutive character", numPasswordsFailedMaxRepeatConsCharTest);
                writer.WriteLine("A password policy requiring a maximum of 2 repeated consecutive characters would result in {0}% of the passwords in this file being forbidden from use", percentPasswordsFailedMaxRepeatConsCharTest); //TODO Replace the hardcoded 2 with a variable conforming to the currently selected password policies designated maximum number of repeated consecutive characters

                //Write the final score
                writer.WriteLine("Summary: Using the currently selected password policy, only {0} passwords from the list would make the cut", numPasswordsPassedALLTests);

                //Close the file after writing
                writer.Close();

                //Write the stats to the console

                //Length stats
                Console.WriteLine("{0} passwords passed the length test", numPasswordsPassedLengthTest);
                Console.WriteLine("{0} out of {1} passwords are insufficient length.", numPasswordsFailedLengthTest, totalNumPasswords);
                Console.WriteLine("A password policy requiring a length of {0} characters would result in {1}% of the passwords in this file being forbidden from use", SelectedPasswordPolicy.MinOverallLength, percentPasswordsFailedLengthTest); //TODO This is not working, figure out how to calculate percentages in C#

                //Max Repeating Cons Chars stats
                Console.WriteLine("{0} passwords passed the test for maximum repeated consecutive characters", numPasswordsPassedMaxRepeatConsCharTest);
                Console.WriteLine("{0} passwords failed the test for maximum repeated consecutive character", numPasswordsFailedMaxRepeatConsCharTest);
                Console.WriteLine("A password policy requiring a maximum of 2 repeated consecutive characters would result in {0}% of the passwords in this file being forbidden from use", percentPasswordsFailedMaxRepeatConsCharTest);

                //Final score:
                Console.WriteLine("Summary: Using the currently selected password policy, only {0} passwords from the list would make the cut", numPasswordsPassedALLTests);
                Console.ReadLine();
        }
    }
}
