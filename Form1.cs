using System;
using System.IO;
using System.Windows.Forms;


namespace CrudXML
{
    public partial class Form1 : Form
    {
        string ruta = "E:\\USAC\\Segundo Semestre 2021\\Inteligencia Artificial 1\\Laboratorio\\Proyecto 2\\source.txt";
        string fileT = "E:\\USAC\\Segundo Semestre 2021\\Inteligencia Artificial 1\\Laboratorio\\Proyecto 2\\fileT.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] respuesta;
            
            respuesta = AgregarEscenaArchivo(textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(),
                textBox4.Text.Trim(), textBox5.Text.Trim(), textBox6.Text.Trim());
            MessageBox.Show(respuesta[1].ToString());

        }
        

        public bool validarPosiciones(string sillon, string mesa, string sofa, string lampara, string jacuzzi) 
        {
            if ((sillon != mesa) && (sillon != sofa) && (sillon != lampara) && (sillon != jacuzzi))
            {
                if ((mesa != sofa) && (mesa != lampara) && (mesa != jacuzzi))
                {
                    if ((sofa != lampara) && (sofa != jacuzzi))
                    {
                        if (lampara != jacuzzi)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public string[] AgregarEscenaArchivo(string nombre, string sillon, string mesa, string sofa, string lampara, string jacuzzi)
        {        
            int numeroLineas = 0;
            char delimitador = ',';
            string[] respuesta;
            respuesta = new string[2];
            foreach (string line in System.IO.File.ReadLines(ruta))
            {
                string[] dato = line.Split(delimitador);
                if (dato[0] == nombre) //Encuentra un espacio con ese piso asignado
                {
                    respuesta[0] = "0";
                    respuesta[1] = "Piso ya asignado a espacio existente";
                    Bitacora("[Error] - " + respuesta[1]);
                    return respuesta; 
                }
                numeroLineas = numeroLineas+1;                
            }

            if (numeroLineas < 7) //6 escenarios más encabezado
            {
                if (validarPosiciones(sillon, mesa, sofa, lampara, jacuzzi))
                {
                    string conf = nombre + "," + sillon + "," + mesa + "," + sofa + "," + lampara + "," + jacuzzi;
                    File.AppendAllText(ruta, conf + Environment.NewLine);
                    respuesta[0] = "1";
                    respuesta[1] = "Espacio " + nombre + " creado con éxito!";
                    Bitacora("[Acción] - " + respuesta[1]);
                    return respuesta;
                }
                else
                {
                    respuesta[0] = "0";
                    respuesta[1] = "Mismo mueble en dos posiciones diferentes";
                    Bitacora("[Error] - " + respuesta[1]);
                    return respuesta;
                }
            }
            else
            {
                respuesta[0] = "0";
                respuesta[1] = "Se intentó registrar más de 6 espacios";
                Bitacora("[Error] - " + respuesta[1]);
            }
            return respuesta;
        }

        public string[] EliminarEscenaArchivo(string nombrePiso)
        {
            string[] respuesta;
            respuesta = new string[2];
            bool eliminado = false;
            char delimitador = ',';
            foreach (string line in System.IO.File.ReadLines(ruta))
            {
                string[] dato = line.Split(delimitador);
                if (dato[0] != nombrePiso) //Me escribe todo en un nuevo archivo, a excepción de lo que quiero eliminar
                {
                    File.AppendAllText(fileT, line + Environment.NewLine);                    
                }
                else
                {
                    eliminado = true;
                }
            }
            File.Delete(ruta);
            File.Move(fileT, ruta);

            if (eliminado)
            {
                respuesta[0] = "1";
                respuesta[1] = "Espacio "+ nombrePiso + " eliminado con éxito";
                Bitacora("[Acción] - " + respuesta[1]);
            }
            else
            {
                respuesta[0] = "0";
                respuesta[1] = "No sé encontró el espacio " + nombrePiso + " para eliminar";
                Bitacora("[Error] - " + respuesta[1]);
            }
            return respuesta;

        }

        public string[] ModificarEscenaArchivo(string nombrePiso, string sillon, string mesa, string sofa, string lampara, string jacuzzi)
        {
            int numeroLineas = 0;
            string[] respuesta;
            respuesta = new string[2];
            char delimitador = ',';
            bool encontrado = false;
            bool modificado = false;
            //bool modificado = false;
            foreach (string line in System.IO.File.ReadLines(ruta))
            {
                string[] dato = line.Split(delimitador);
                if (dato[0] == nombrePiso) //Encuentra un espacio con ese piso asignado
                {
                    encontrado = true;
                }
                numeroLineas = numeroLineas + 1;
            }
            //Validación de número de escenas
            if (numeroLineas >= 7) //6 escenarios más encabezado
            {
                respuesta[0] = "0";
                respuesta[1] = "Se tienen 6 espacios creados, elimine un espacio para poder modificar";
                Bitacora("[Error] - " + respuesta[1]);
                return respuesta;
            }
            //Validación de si encontro el archivo a modificar
            if (!encontrado)
            {
                respuesta[0] = "0";
                respuesta[1] = "No sé encontro el piso "+ nombrePiso + " para modificar";
                Bitacora("[Error] - " + respuesta[1]);
                return respuesta;
            }


            if (validarPosiciones(sillon, mesa, sofa, lampara, jacuzzi))
            {
                foreach (string line in System.IO.File.ReadLines(ruta))
                {
                    string[] dato = line.Split(delimitador);
                    if (dato[0] != nombrePiso) //Me escribe todo en un nuevo archivo, a excepción de lo que quiero eliminar
                    {
                        File.AppendAllText(fileT, line + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(fileT, nombrePiso + "," + sillon + "," + mesa + "," +
                            sofa + "," + lampara + "," + jacuzzi + Environment.NewLine);
                        modificado = true;
                    }
                }
                File.Delete(ruta);
                File.Move(fileT, ruta);
            }
            else
            {
                respuesta[0] = "0";
                respuesta[1] = "Mismo mueble en dos posiciones diferentes";
                Bitacora("[Error] - " + respuesta[1]);
                return respuesta;
            }

            if (modificado)
            {
                respuesta[0] = "1";
                respuesta[1] = "Espacio " + nombrePiso + " modificado con éxito!";
                Bitacora("[Acción] - " + respuesta[1]);
                return respuesta;
            }




            /*foreach (string line in System.IO.File.ReadLines(ruta))
            {
                
                string[] dato = line.Split(delimitador);
                if (dato[0] != nombrePiso) //Me escribe todo en un nuevo archivo, a excepción de lo que quiero eliminar
                {
                    File.AppendAllText(fileT, line + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText(fileT, nombrePiso + "," +sillon +"," + mesa + "," + 
                        sofa + "," + lampara + "," + jacuzzi + Environment.NewLine);
                    modificado = true;
                }
                
            }
            File.Delete(ruta);
            File.Move(fileT, ruta);

            if (modificado)
            {
                respuesta[0] = "1";
                respuesta[1] = "Espacio " + nombrePiso + " modificado con éxito";
                Bitacora("[Acción] - " + respuesta[1]);
            }
            else
            {
                respuesta[0] = "0";
                respuesta[1] = "No sé encontró el espacio " + nombrePiso + " para modificar";
                Bitacora("[Error] - " + respuesta[1]);
            }*/
            return respuesta;

        }

        public string[] VerEscenaArchivo(string nombrePiso)
        {
            string[] respuesta;
            respuesta = new string[8];
            bool encontrado = false;
            char delimitador = ',';
            foreach (string line in System.IO.File.ReadLines(ruta))
            {
                string[] dato = line.Split(delimitador);
                if (dato[0] == nombrePiso)
                {
                    encontrado = true;
                    respuesta[0] = "1";
                    respuesta[1] = "Espacio " + nombrePiso + " cargado con éxito";
                    respuesta[2] = dato[0];
                    respuesta[3] = dato[1];
                    respuesta[4] = dato[2];
                    respuesta[5] = dato[3];
                    respuesta[6] = dato[4];
                    respuesta[7] = dato[5];
                    Bitacora("[Acción] - " + respuesta[1]);
                }                
            }
            
            if (!encontrado)
            {
                encontrado = false;
                respuesta[0] = "0";
                respuesta[1] = "No sé encontró el espacio " + nombrePiso + " para cargar";
                Bitacora("[Error] - " + respuesta[1]);
            }           
            return respuesta;

        }

        public void Bitacora(string linea)
        {
            string ruta = "E:\\USAC\\Segundo Semestre 2021\\Inteligencia Artificial 1\\Laboratorio\\Proyecto 2\\bitacora.txt";
            //string ruta = Application.dataPath + "/bitacora.txt";
            //Se coloca en metodo start de Unity
            if (!File.Exists(ruta))
            {
                File.WriteAllText(ruta, "BITACORA IA1 2S 2021" + Environment.NewLine);
                File.AppendAllText(ruta, DateTime.Now.ToString() +" " +linea + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(ruta, DateTime.Now.ToString()+ " "+ linea + Environment.NewLine);
            }
         }

        private void button2_Click(object sender, EventArgs e)
        {         
            string[] respuesta;
            respuesta = EliminarEscenaArchivo(textBox1.Text);
            MessageBox.Show(respuesta[1].ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] respuesta;
            respuesta = ModificarEscenaArchivo(textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(),
                textBox4.Text.Trim(), textBox5.Text.Trim(), textBox6.Text.Trim());
            MessageBox.Show(respuesta[1].ToString());            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] respuesta;

            respuesta = VerEscenaArchivo(textBox1.Text.Trim());
            MessageBox.Show(respuesta[1].ToString());
            if (respuesta[0] == "1") { 
                textBox1.Text = respuesta[2];
                textBox2.Text = respuesta[3];
                textBox3.Text = respuesta[4];
                textBox4.Text = respuesta[5];
                textBox5.Text = respuesta[6];
                textBox6.Text = respuesta[7];
            }

        }
    }
}
