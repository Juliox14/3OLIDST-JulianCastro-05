using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace _3OLIDST_JulianCastro_005
{
    //Desactivar advertencias innecesarias
#pragma warning disable CS8622
    public partial class Form1 : Form
    {
        string conexionSQL = "Server=localhost;Port=3306;Database=Formulario_Datos;Uid=root;Pwd=martinteto04;";
        bool valid = false;
        public Form1()
        {
            InitializeComponent();
            txtEdad.TextChanged += ValidarEdad;
            txtEstatura.TextChanged += ValidarEstatura;
            txtNombre.TextChanged += ValidarNombre;
            txtApellido.TextChanged += ValidarApellido;
        }
        private void InsertarRegistro(string nombre, string apellido, string telefono, decimal estatura, int edad, string genero)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSQL))
            {
                conection.Open();

                string insertQuery = "INSERT INTO datos (Nombre, Apellidos, Telefono, Estatura, Edad, Genero) " +
                    "VALUES (@Nombre, @Apellidos, @Telefono, @Estatura, @Edad, @Genero)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellidos", apellido);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Estatura", estatura);
                    command.Parameters.AddWithValue("@Edad", edad);
                    command.Parameters.AddWithValue("@Genero", genero);

                    command.ExecuteNonQuery();

                }
                conection.Close();
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            valid = false;
            // Limpiar los controles despu�s de guardar
            txtNombre.Clear();
            txtApellido.Clear();
            txtEstatura.Clear();
            txtTelefono.Clear();
            txtEdad.Clear();
            rbtnHombre.Checked = false;
            rbtnMujer.Checked = false;
        }
       
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // obtener los datos de los TextBox
            valid = true;

            string nombres = txtNombre.Text;
            string apellidos = txtApellido.Text;
            string edad = txtEdad.Text;
            string estatura = txtEstatura.Text;
            string telefono = txtTelefono.Text;
            string genero = "";

            if (rbtnHombre.Checked == true)
            {
                genero = "Hombre";

            }
            else if (rbtnMujer.Checked == true)
            {
                genero = "Mujer";
            }

            if (EsEnteroValido(edad) && EsDecimalValido(estatura) && EsTextoValido(nombres) && EsTextoValido(apellidos))
            {
                // Validar longitud del n�mero de tel�fono
                if (telefono.Length == 10 && EsEnteroValido10Digitos(telefono))
                {
                    string datos = "Datos guardados con �xito: \nNombre : " + nombres + "\nApellido : " + apellidos + "\nEdad : " + edad + 
                        "\nEstatura : " + estatura + "\nTelefono : " + telefono + "\nGenero : " + genero + "\n";
                    MessageBox.Show(datos);
                    string rutaArchivo = "C:/Users/pooll/Documents/Datos.txt";
                    bool archivoExiste = File.Exists(rutaArchivo);
                    if (archivoExiste == false)
                    {
                        File.WriteAllText(rutaArchivo, datos);
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                        {
                            if (archivoExiste)
                            {

                                writer.WriteLine();
                                InsertarRegistro(nombres, apellidos, telefono, decimal.Parse(estatura), int.Parse(edad), genero);
                                Console.WriteLine("Datos ingresados correctamente");
                            }
                            else
                            {
                                writer.WriteLine(datos);
                                InsertarRegistro(nombres, apellidos, telefono, decimal.Parse(estatura), int.Parse(edad), genero);
                                Console.WriteLine("Datos ingresados correctamente");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese un n�mero de tel�fono v�lido de 10 d�gitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese datos v�lidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }

        private bool EsEnteroValido10Digitos(string input)
        {
            if (input.Length != 10)
            {
                return false;
            }
            if (!input.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            if (valid == true) 
            { 
                TextBox textbox = (TextBox)sender;
                if (!EsEnteroValido(textbox.Text))
                {
                    MessageBox.Show("Por favor ingrese una edad v�lida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textbox.Clear();
                }
            }
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            if (valid == true)
            {
                TextBox textbox = (TextBox)sender;
                if (!EsDecimalValido(textbox.Text))
                {
                    MessageBox.Show("Por favor ingrese una estatura v�lida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textbox.Clear();
                }
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            if (valid == true)
            {
                TextBox textbox = (TextBox)sender;
                if (!EsTextoValido(textbox.Text))
                {
                    MessageBox.Show("Por favor ingrese un nombre v�lido (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textbox.Clear();
                }
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            if (valid == true)
            {
                TextBox textbox = (TextBox)sender;
                if (!EsTextoValido(textbox.Text))
                {
                    MessageBox.Show("Por favor ingrese apellidos v�lidos (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textbox.Clear();
                }
            }
        }

    }
}