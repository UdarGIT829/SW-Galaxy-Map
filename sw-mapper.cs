using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

public class GalMap : Form
{
    Label Control1;
    TextBox inputSystem,inputSystemLetter,inputSector;
    //PictureBox imageControl;
    Dictionary< Tuple<int,int> ,string> systemDict = new Dictionary< Tuple<int,int> ,string>();
    
    //Multi-use global variables
    bool inputReadyCheck = false;

    public void populateSystemDict ()
    {
        systemDict = new Dictionary< Tuple<int,int> ,string>();
        int xCoor, yCoor;
        string name;
        Tuple<int,int> coords;
        char lastLetter;

        string[] lines = System.IO.File.ReadAllLines("sector-text");
        foreach (string line in lines)
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
            inputSector.Text = tokens[4];
            coords = new Tuple<int, int>(xCoor,yCoor);
            systemDict.Add(coords,name);
        }
    }

    public string searchSystemDict( Tuple<int,int> inCoords )
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
        populateSystemDict();

        PictureBox mapControl = new PictureBox();  
        Bitmap source = new Bitmap("starwars_map.JPG");  
        mapControl.Size = source.Size;
        mapControl.Location = new Point(0,5 + Control1.PreferredHeight+5);
        this.Size = source.Size;// + new Size(30, 30);
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
        string searchResult = searchSystemDict(new Tuple<int,int>(e.X,e.Y));
        messageBoxCS.AppendFormat("{0} = {1}", "Search", searchResult );
        messageBoxCS.AppendLine();
        if(inputReadyCheck && inputSystem.Text != "" && searchResult == "Not Found")
        {
            using (StreamWriter w = File.AppendText("sector-text"))
            {
                w.WriteLine("{0},{1},{2},{3},{4}",e.X,e.Y,inputSystem.Text,inputSystemLetter.Text,inputSector.Text);
            }
            messageBoxCS.AppendFormat("New Star System Added!");
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0},{1},Sector: {2}",inputSystem.Text,inputSystemLetter.Text,inputSector.Text);
            messageBoxCS.AppendLine();
            populateSystemDict();
            inputSystem.Text = "";
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
        }
        else
        {
            Control1.Text = "Sourced Map";
            inputReadyCheck = false;
            inputSystem.Hide();
            inputSystemLetter.Hide();
            inputSector.Hide();
        }
        messageBoxCS.AppendFormat("1st Textbox = System Name, 2nd = Letter(Optional), 3rd = Sector Name");
        messageBoxCS.AppendLine();
        MessageBox.Show(messageBoxCS.ToString(), "User Input Ready!" );
    }

}