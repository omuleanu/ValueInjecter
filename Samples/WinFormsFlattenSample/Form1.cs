using System;
using System.Windows.Forms;
using Omu.ValueInjecter;

namespace WinFormsFlattenSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var person =
                new Person
                    {
                        Name = "Vadim",
                        Age = 13,
                        BirthDate = DateTime.Now,
                        HomeAddress = new Address
                                         {
                                             HouseNumber = 113,
                                             Street = "HollyFlatten"
                                         },
                        DriverLicense = new Document
                                            {
                                                Number = "11134432112",
                                                Office = new IssueOffice
                                                             {
                                                                 Name = "Gordon office",
                                                                 Address = new Address
                                                                               {
                                                                                   HouseNumber = 233,
                                                                                   Street = "NotronHood"
                                                                               }
                                                             }
                                            }
                    };

            this.InjectFrom<StringToTextBox>(person)
                .InjectFrom<DateTimeToDateTimePicker>(person);


            var pf = new PersonFlat();
            pf.InjectFrom<SameNameFlat>(person);

            var pp = new Person();
            pp.InjectFrom<SameNameUnflat>(pf);
            LoadList();
        }

        private void BtInjectClick(object sender, EventArgs e)
        {
            LoadList();
        }
        private void LoadList()
        {
            var person = new Person();

            person.InjectFrom<TextBoxToString>(this)
                .InjectFrom<DateTimePickerToDateTime>(this);
            
            lstBox.Items.Clear();
            lstBox.Items.Add("person.Name - "+person.Name);
            lstBox.Items.Add("person.Age - " + person.Age);
            lstBox.Items.Add("person.BirthDate - " + person.BirthDate);
            lstBox.Items.Add("person.HomeAddress.Street - " + person.HomeAddress.Street);
            lstBox.Items.Add("person.HomeAddress.HouseNumber - " + person.HomeAddress.HouseNumber);
            lstBox.Items.Add("person.DriverLicense.Number - " + person.DriverLicense.Number);
            lstBox.Items.Add("person.DriverLicense.Office.Name - " + person.DriverLicense.Office.Name);
            lstBox.Items.Add("person.DriverLicense.Office.Address.Street - " + person.DriverLicense.Office.Address.Street);
            lstBox.Items.Add("person.DriverLicense.Office.Address.HouseNumber - " + person.DriverLicense.Office.Address.HouseNumber);
        }
    }
}