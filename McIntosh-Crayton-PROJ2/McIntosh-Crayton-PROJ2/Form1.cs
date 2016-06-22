/*
Developer: McIntosh, Cryaton  
Course: MIS 4321 – Spring 2016
Assignment: Project #2 - Nutrition Planner  
Description: This project pulls information from 2 different arrays to be chosen from by the user, using the buttons 
             on the form. On each button click the form will then update the nutritional summary table by adding 
             the food as well as its nutrient information. If the user inputs goals, the form will prompt the user
             that he/she has gone over that goal, and you can suggest an item 

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace McIntosh_Crayton_PROJ2
{
    public partial class Form1 : Form
    {
        //array declaration for the food items and their nutritional goals
        string[] menuItems = new string[17] {"Quarter Lb Burger", "Third Lb Burger", "Half Lb Burger", "Club Sandwich", 
                                             "Grilled Chicken", "Philly Steak Sandwich", "Chicken Wrap", "Small Fries", 
                                             "Medium Fries", "Large Fries", "Egg Roll", "Fruit Cup", "Cookies", "Cola", 
                                             "Diet Cola", "Ice Cream Shake", "Apple Juice"};
        int[,] nutritionalValues = new int[17, 4] { {300, 33, 20, 9}, {510, 40, 29, 26}, {790, 63, 45, 39}, {420, 51, 32, 9}, 
                                                    {400, 39, 24, 16}, {410, 27, 20, 24}, {160, 31, 4, 2}, {230, 30, 2, 11},
                                                    {340, 8, 4, 16}, {510, 12, 6, 24}, {260, 43, 4, 8}, {70, 15, 0, 1},
                                                    {380, 48, 4, 19}, {220, 59, 0, 0}, {1, 0, 0, 0}, {210, 52, 0, 0}, {100, 23, 0, 0}};
        //declaring global variables that will increment per button click
        int totalCalories = 0,
            totalCarbs = 0,
            totalProtein = 0,
            totalFat = 0,
            goalCalories = 0,
            goalCarbs = 0,
            goalFat = 0,
            goalProtein = 0,
            calorieDiff = 0,
            carbDiff = 0,
            proteinDiff = 0,
            fatDiff = 0;
        int isInt;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //loads the menu buttons from the array
            LoadMenuButtons(); 
            //makes the suggest item button disabled when the form loads
            btnSuggestItem.Enabled = false;
        }

        private void LoadMenuButtons()
        { 
            //data declaration count for all the groupboxes
            int count;
            //sandwiches groupbox
            foreach(Control ctrl in groupBoxSandwiches.Controls)
            {
                if (ctrl is Button)
                {
                    count = Convert.ToInt32(ctrl.Tag) - 1;
                    ctrl.Name = menuItems[count];
                    ctrl.Text = menuItems[count];
                } 
            }
            //sides groupbox
            foreach(Control ctrl in groupBoxSides.Controls)
            {
                if (ctrl is Button)
                {
                    count = Convert.ToInt32(ctrl.Tag) - 1;
                    ctrl.Name = menuItems[count];
                    ctrl.Text = menuItems[count];
                }
            }
            //drinks groupbox
            foreach (Control ctrl in groupBoxDrinks.Controls)
            {
                if (ctrl is Button)
                {
                    count = Convert.ToInt32(ctrl.Tag) - 1;
                    ctrl.Name = menuItems[count];
                    ctrl.Text = menuItems[count];
                }
            }
        }

        private void btnClearSelections_Click(object sender, EventArgs e)
        {
            //clears the totals in the nutritional goals section
            txtTotalCalories.Clear();
            txtTotalCarbs.Clear();
            txtTotalFat.Clear();
            txtTotalProtein.Clear();

            dataGridViewNutritionSummaryTable.Rows.Clear();
            
            //clears the variables that increment based on each button click
            totalCalories = 0;
            totalCarbs = 0;
            totalFat = 0;
            totalProtein = 0;

            ResetFlags();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //clears Nutritional Goals
            txtCalorieGoals.Clear();
            txtCarbsGoals.Clear();
            txtFatGoals.Clear();
            txtProteinGoals.Clear();
            
            //clears totals in the nutritional goals section
            txtTotalCalories.Clear();
            txtTotalCarbs.Clear();
            txtTotalFat.Clear();
            txtTotalProtein.Clear();

            dataGridViewNutritionSummaryTable.Rows.Clear();

            //clears the variables that increment based on each button click
            totalCalories = 0;
            totalCarbs = 0;
            totalFat = 0;
            totalProtein = 0;

            ResetFlags();
        }

        private void AddMenuSelection(object sender, EventArgs e)
        {
            if (txtTotalCalories.BackColor != Color.Red && txtTotalCarbs.BackColor != Color.Red &&
                txtTotalFat.BackColor != Color.Red && txtTotalProtein.BackColor != Color.Red)
            {
                //adds info to the data grid view
                Button btn = (Button)sender;
                int tagNumber = Convert.ToInt32(btn.Tag) - 1;
                this.dataGridViewNutritionSummaryTable.Rows.Add(menuItems[tagNumber], nutritionalValues[tagNumber, 0],
                                                                                      nutritionalValues[tagNumber, 1],
                                                                                      nutritionalValues[tagNumber, 2],
                                                                                      nutritionalValues[tagNumber, 3]);
                //adds the nutritional values to the appropriate textbox
                totalCalories += nutritionalValues[tagNumber, 0];
                totalCarbs += nutritionalValues[tagNumber, 1];
                totalProtein += nutritionalValues[tagNumber, 2];
                totalFat += nutritionalValues[tagNumber, 3];

                txtTotalCalories.Text = Convert.ToString(totalCalories);
                txtTotalCarbs.Text = Convert.ToString(totalCarbs);
                txtTotalProtein.Text = Convert.ToString(totalProtein);
                txtTotalFat.Text = Convert.ToString(totalFat);

                CompareGoals();
            }
        }

        private void CompareGoals()
        { 
            //comparing textboxes of Calories
            if (!string.IsNullOrEmpty(txtCalorieGoals.Text)) 
            {
                //int cal = Convert.ToInt32(txtCalorieGoals.Text);
                if (int.TryParse(txtCalorieGoals.Text, out isInt) == true)
                {
                    goalCalories = Convert.ToInt32(txtCalorieGoals.Text);
                    if (goalCalories < totalCalories)
                    {
                        txtTotalCalories.BackColor = Color.Red;
                        //enables the suggest item button
                        btnSuggestItem.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a number!");
                }
            }
            //comparing textboxes of Carbs
            if (!string.IsNullOrEmpty(txtCarbsGoals.Text)) 
            {
                if (int.TryParse(txtCarbsGoals.Text, out isInt) == true)
                {
                    goalCarbs = Convert.ToInt32(txtCarbsGoals.Text);
                    if (goalCarbs < totalCarbs)
                    {
                        txtTotalCarbs.BackColor = Color.Red;
                        btnSuggestItem.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a number!");
                }
            }
            //comparing textboxes of Protein
            if (!string.IsNullOrEmpty(txtProteinGoals.Text) && !string.IsNullOrEmpty(txtTotalProtein.Text))
            {
                if (int.TryParse(txtProteinGoals.Text, out isInt) == true)
                {
                    goalProtein = Convert.ToInt32(txtProteinGoals.Text);
                    if (goalProtein < totalProtein)
                    {
                        txtTotalProtein.BackColor = Color.Red;
                        btnSuggestItem.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a number!");
                }
            }
            //comparing textboxes of Fats
            if (!string.IsNullOrEmpty(txtFatGoals.Text) && !string.IsNullOrEmpty(txtTotalFat.Text))
            {
                if (int.TryParse(txtFatGoals.Text, out isInt) == true)
                {
                    goalFat = Convert.ToInt32(txtFatGoals.Text);
                    if (goalFat < totalFat)
                    {
                        txtTotalFat.BackColor = Color.Red;
                        btnSuggestItem.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a number!");
                }        
            }
        }

        private void ResetFlags()
        {
            //resets all the colors of the text boxes
            txtTotalCalories.BackColor = Color.White;
            txtTotalCarbs.BackColor = Color.White;
            txtTotalFat.BackColor = Color.White;
            txtTotalProtein.BackColor = Color.White;
        }

        private void txtCalorieGoals_Leave(object sender, EventArgs e)
        {
            //resets and compares every time the user leaves this box
            ResetFlags();
            CompareGoals();
        }

        private void txtCarbsGoals_Leave(object sender, EventArgs e)
        {            
            //resets and compares every time the user leaves this box
            ResetFlags();
            CompareGoals();
        }

        private void txtProteinGoals_Leave(object sender, EventArgs e)
        {
            //resets and compares every time the user leaves this box
            ResetFlags();
            CompareGoals();
        }

        private void txtFatGoals_Leave(object sender, EventArgs e)
        {
            //resets and compares every time the user leaves this box
            ResetFlags();
            CompareGoals();
        }

        //suggest item button, can't get it to work exactly right
        private void btnSuggestItem_Click(object sender, EventArgs e)
        {
            calorieDiff = totalCalories - goalCalories;
            carbDiff = totalCarbs - goalCarbs;
            proteinDiff = totalProtein - goalProtein;
            fatDiff = totalFat - goalFat;

            if (txtTotalCalories.BackColor == Color.Red)
            {
                for (int i = 0; i < nutritionalValues.Length; ++i)
                {
                    if (nutritionalValues[i, 0] < calorieDiff)
                    {
                        MessageBox.Show(menuItems[i] + ": " + nutritionalValues[i, 0]);
                    }
                }
            }

            if (txtTotalCarbs.BackColor == Color.Red)
            {
                for (int i = 0; i < nutritionalValues.Length; ++i)
                {
                    if (nutritionalValues[i, 0] < carbDiff)
                    {
                        MessageBox.Show(menuItems[i] + ": " + nutritionalValues[i, 0]);
                    }
                }
            }

            if(txtTotalProtein.BackColor == Color.Red)
            {
                for (int i = 0; i < nutritionalValues.Length; ++i)
                {
                    if (nutritionalValues[i, 0] < proteinDiff)
                    {
                        MessageBox.Show(menuItems[i] + ": " + nutritionalValues[i, 0]);
                    }
                }
            }

            if(txtTotalFat.BackColor == Color.Red)
            {
                for (int i = 0; i < nutritionalValues.Length; ++i)
                {
                    if (nutritionalValues[i, 0] < fatDiff)
                    {
                        MessageBox.Show(menuItems[i] + ": " + nutritionalValues[i, 0]);
                    }
                }
            }
        }
    }
}
