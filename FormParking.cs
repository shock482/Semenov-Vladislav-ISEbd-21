﻿using laba2sem1;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2sem1
{
    public partial class FormParking : Form
    {
        
        Parking parking;
        FormSelectRock form;
        private Logger log;
        public FormParking()
        {
            InitializeComponent();
           log = LogManager.GetCurrentClassLogger(); 
            parking = new Parking(5);

            for (int i = 1; i < 6; i++)
            {
                listBox1.Items.Add("Уровень " + i);
            }
            listBox1.SelectedIndex = parking.getCurrentLevel;
            Draw();
        }

        private void Draw()
        {
            if (listBox1.SelectedIndex > -1)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics gr = Graphics.FromImage(bmp);
                parking.Draw(gr);
                pictureBox1.Image = bmp;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AddRock(IRock rock)
        {
            if (rock != null)
            {
                try
                { 
                int place = parking.PutRockInParking(rock);
                    Draw();
                    MessageBox.Show("Ваше место: " + place);
                }
                catch (ParkingOverflowException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка переполнения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ParkingAlreadyHaveException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка совпадения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Общая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                      
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ColorDialog dialogDop = new ColorDialog();
                if (dialogDop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var rock = new Diamond(100, 5, 200, dialog.Color, true, true, dialogDop.Color);
                    int place = parking.PutRockInParking(rock);
                    Draw();
                    MessageBox.Show("Ваша полка: " + place);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                string level = listBox1.Items[listBox1.SelectedIndex].ToString();
                if (maskedTextBox1.Text != "")
                {
                    try
                    { 
                    IRock rock = parking.GetRockInParking(Convert.ToInt32(maskedTextBox1.Text));
                    
                        Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                        Graphics gr = Graphics.FromImage(bmp);
                        rock.setPosition(15, 15);
                        rock.drawRock(gr);
                        pictureBox2.Image = bmp;
                        Draw();
                    
                    }
                    catch(ParkingIndexOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message, "Неверный номер", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Общая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                log.Info("Нажатие кнопки купить ");
            }        
        }

        private event myDel eventAddRock;

        private void AddEvent(myDel ev)
        {
            if (eventAddRock == null)
            {
                eventAddRock = new myDel(ev);
            }
            else
            {
                eventAddRock += ev;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            parking.LevelDown();
            listBox1.SelectedIndex = parking.getCurrentLevel;
            log.Info("Переход на уровень ниже Текущий уровень: " + parking.getCurrentLevel);
            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            parking.LevelUp();
            listBox1.SelectedIndex = parking.getCurrentLevel;
            log.Info("Переход на уровень выше Текущий уровень: " + parking.getCurrentLevel);
            Draw();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            form = new FormSelectRock();
            form.AddEvent(AddRock);
            form.Show();
            log.Info("Нажатие на заказ драгоценности ");
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(parking.SaveData(saveFileDialog1.FileName))
                {
                    MessageBox.Show("Сохранение прошло успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не соранилось", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (parking.LoadData(openFileDialog1.FileName))
                {
                    MessageBox.Show("Загрузили", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не загрузили", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Draw();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            parking.Sort();
            Draw();
            log.Info("Сортировка уровня " + parking.getCurrentLevel);
        }
    }
}