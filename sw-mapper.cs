using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace StarMap
{
    public class GalMap : Form
    {
        Label Control1;
        TextBox inputSystem,inputSystemLetter,inputSector;
        //PictureBox imageControl;
        Dictionary< Tuple<int,int> ,string > systemDict = new Dictionary< Tuple<int,int> ,string>();
        Dictionary< string , sSystemDetails > systemDetailsDict = new Dictionary< string ,sSystemDetails>();

        Dictionary< string, List<string> > regionDict = new Dictionary<string, List<string> >();
        //Multi-use global variables

        
        bool inputReadyCheck = false;

        /*
        public void populateRegionList() //This is the Problem
        {
            string region; 
            string tempSector;
            string[] tempSectorArr;
            List<string> tempSectors;
            string[] sectorLines = System.IO.File.ReadAllLines("sector-text");
            foreach (string sector in sectorLines)
            {
                tempSectorArr = sector.Split(',');
                region = tempSectorArr[0];
                tempSectors = new List<string>();
                for(int i=1;i<sectorLines.Length;i++)
                {
                    tempSector = tempSectorArr[i]; 
                    tempSectors.Add(tempSector);
                }
                regionDict.Add(region, tempSectors);
            }

        }
        */
        public void populateVisibleSystemDict ()
        {
            //systemDict = new Dictionary< Tuple<int,int> ,string>();
            int xCoor, yCoor;
            string name;
            Tuple<int,int> coords;
            char lastLetter;
            string tempSector;
            sSystemDetails temp;

            //populateRegionList();
            string[] systemLines = System.IO.File.ReadAllLines("sector-text");
            foreach (string line in systemLines)
            {
                if(line == "_end_")
                {
                    break;
                }
                string[] tokens = line.Split(',');
                xCoor = int.Parse(tokens[0]);
                yCoor = int.Parse(tokens[1]);
                name = tokens[2];
                lastLetter = ((Char)(Convert.ToUInt16(tokens[3][0])+1));
                inputSystemLetter.Text = lastLetter.ToString();
                tempSector = tokens[4];
                inputSector.Text = tempSector;
                coords = new Tuple<int, int>(xCoor,yCoor);
                systemDict.Add(coords,name);
                temp = new sSystemDetails(name,coords,tempSector);
                if(!systemDetailsDict.ContainsKey(name))
                {
                    systemDetailsDict.Add(name,temp);
                }
            }
        }

        public string searchRegionsBySector (string searchSector)
        {
            string result = "Not found";
            foreach(KeyValuePair<string, List<string> > region in regionDict)
            {
                if(region.Value.Exists(x => x == searchSector))
                {
                    result = region.Key;
                    break;
                }
            }
            return result;

        }
        public string searchCoordsSystemDict( Tuple<int,int> inCoords )
        {
            string result;

            int diffX, diffY, distance;
            int minDist = int.MaxValue; //set at max to find minimum distance
            //find closest system
            foreach(KeyValuePair< Tuple<int,int> ,string>sSystem in systemDict)
            {
                diffX = Math.Abs(sSystem.Key.Item1 - inCoords.Item1);
                diffY = Math.Abs(sSystem.Key.Item2 - inCoords.Item2);
                distance = diffX + diffY;
                if(distance < minDist && distance < 6)
                {
                    minDist = distance;
                    inCoords = sSystem.Key;
                }
            }

            try
            {
                result = systemDict[inCoords];
                Console.WriteLine(result);
            }
            catch (KeyNotFoundException)
            {
                result = "Not Found";
                if(inputReadyCheck && inputSector.Text != "") //Ignores the second logic in this if() for some reason
                {
                    Console.WriteLine("{0} added to system {1}!",inputSystem.Text,inputSector.Text);
                }
                else
                {
                    Console.WriteLine("Not Found");
                }
            }
            return result;
        }

        public string searchDetailsDict( string inputS )
        {
            string result;
            try
            {
                result = systemDetailsDict[inputS].sectorName;
            }
            catch (KeyNotFoundException)
            {
                result = "No description.";
            }
            return result;
        }

        static public void Main ()
        {
            Application.Run (new GalMap ());
        }

        public GalMap ()
        {
            //Multi-use variable flags

            this.Text = "Star-Map";
            Control1 = new Label();  
            Control1.UseMnemonic = true;  
            Control1.Text = "Sourced Map";  
            Control1.Location = new Point(5, 5);  
            Control1.BackColor = Color.Pink;  
            Control1.ForeColor = Color.Maroon;  
            Control1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;  
            Control1.Size = new Size(Control1.PreferredWidth, Control1.PreferredHeight + 2);  
            Control1.MouseClick += mapSwitch_MouseClick;        
            this.Controls.Add(Control1);

            inputSystem = new TextBox();
            inputSystem.BackColor = Color.White;  
            inputSystem.ForeColor = Color.Black;  
            inputSystem.Text = "";  
            inputSystem.Font = new Font("Georgia", 10); 
            inputSystem.Location = new Point(Control1.PreferredWidth+10, 3);
            Controls.Add(inputSystem);
            inputSystem.Hide();

            inputSystemLetter = new TextBox();
            inputSystemLetter.BackColor = Color.White;  
            inputSystemLetter.ForeColor = Color.Black;  
            inputSystemLetter.Text = "";  
            inputSystemLetter.Font = new Font("Georgia", 10); 
            inputSystemLetter.Width = 20;
            inputSystemLetter.Location = new Point(Control1.PreferredWidth+inputSystem.Width+10, 3);
            Controls.Add(inputSystemLetter);
            inputSystemLetter.Hide();

            inputSector = new TextBox();
            inputSector.BackColor = Color.White;  
            inputSector.ForeColor = Color.Black;  
            inputSector.Text = "";  
            inputSector.Font = new Font("Georgia", 10); 
            inputSector.Location = new Point(Control1.PreferredWidth+inputSystem.Width+inputSystemLetter.Width+10, 3);
            Controls.Add(inputSector);
            inputSector.Hide();
            populateVisibleSystemDict();

            PictureBox mapControl = new PictureBox();  
            Bitmap source = new Bitmap("starwars_map.JPG");  
            mapControl.Size = source.Size;
            mapControl.Location = new Point(10,5 + Control1.PreferredHeight+5);
            this.Size = source.Size + new Size(30, 80);
            mapControl.Dock = DockStyle.None;  
            mapControl.Image = (Image) source;  
            Controls.Add(mapControl);  
            mapControl.MouseClick += Control1_MouseClick;
        }
    
        private void Control1_MouseClick(Object sender, MouseEventArgs e) 
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "X", e.X );
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Y", e.Y );
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Location", e.Location );
            messageBoxCS.AppendLine();
            string searchResult = searchCoordsSystemDict(new Tuple<int,int>(e.X,e.Y));
            messageBoxCS.AppendFormat("{0} = {1}", "Search", searchResult );
            messageBoxCS.AppendLine();
            if(inputReadyCheck && inputSystem.Text != "" && searchResult == "Not Found")
            {
                //if coords are not found, add the system using populateSystemDict()
                using (StreamWriter w = File.AppendText("sector-text"))
                {
                    w.WriteLine("{0},{1},{2},{3},{4}",e.X,e.Y,inputSystem.Text,inputSystemLetter.Text,inputSector.Text);
                }
                messageBoxCS.AppendFormat("New Star System Added!");
                messageBoxCS.AppendLine();
                messageBoxCS.AppendFormat("{0},{1},Sector: {2}",inputSystem.Text,inputSystemLetter.Text,inputSector.Text);
                messageBoxCS.AppendLine();
                populateVisibleSystemDict();
                inputSystem.Text = "";
            }
            else if(searchResult == "Not Found")
            {
                //coords not found, do nothing
            }
            else
            {
                //if coords are found, search for details using searchDetailsDict(originalSearchResult)
                string sector = searchDetailsDict(searchResult);
                messageBoxCS.AppendFormat("Sector: {0}",sector);
                messageBoxCS.AppendLine();
                string region = searchRegionsBySector(sector);
                messageBoxCS.AppendFormat("Region: {0}",region);
                messageBoxCS.AppendLine();
            }
            MessageBox.Show(messageBoxCS.ToString(), "MouseClick Event" );

        }

        private void mapSwitch_MouseClick(Object sender, MouseEventArgs e) 
        {
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            if(Control1.Text == "Sourced Map")
            {
                Control1.Text = "User Input";
                inputReadyCheck = true;
                inputSystem.Show();
                inputSystemLetter.Show();
                inputSector.Show();
                messageBoxCS.AppendFormat("1st Textbox = System Name, 2nd = Letter(Optional), 3rd = Sector Name");
                messageBoxCS.AppendLine();

            }
            else
            {
                Control1.Text = "Sourced Map";
                inputReadyCheck = false;
                inputSystem.Hide();
                inputSystemLetter.Hide();
                inputSector.Hide();
                messageBoxCS.AppendFormat("User input disabled");
                messageBoxCS.AppendLine();
            }
            MessageBox.Show(messageBoxCS.ToString(), "User Input Message!" );
        }

    }
}