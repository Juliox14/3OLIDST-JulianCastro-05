using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace _3OLIDST_JulianCastro_005
{
    //Desactivar advertencias innecesarias
    #pragma warning disable CS8622
    public partial class Form1 : Form
    {
        string conexionSQL = "Server=localhost;Port==3306;Database=Formulario_Datos";

        public Form1()
        {
            InitializeComponent();
            txtEdad.TextChanged += ValidarEdad;
            txtEstatura.TextChanged += ValidarEstatura;
            txtNombre.TextChanged += ValidarNombre;
            txtApellido.TextChanged += ValidarApellido;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Limpiar los controles después de guardar
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
                // Validar longitud del número de teléfono
                if (telefono.Length == 10 && EsEnteroValido10Digitos(telefono))
                {
                    string datos = "Datos guardados con éxito: \nNombre : " + nombres + "\nApellido : " + apellidos + "\nEdad : " + edad + "\nEstatura : " + estatura + "\nTelefono : " + telefono + "\nGenero : " + genero + "\n";
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
                                writer.WriteLine(datos);
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese un número de teléfono válido de 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                MessageBox.Show("Por favor ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private bool EsEnteroValido10Digitos(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!EsEnteroValido(textbox.Text))
            {
                MessageBox.Show("Por favor ingrese una edad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textbox.Clear();
            }
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!EsDecimalValido(textbox.Text))
            {
                MessageBox.Show("Por favor ingrese una estatura válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textbox.Clear();
            }
        }

        //private void ValidarTelefono(object sender, EventArgs e)
        //{
        //    TextBox textbox = (TextBox)sender;
        //    string input = textbox.Text;

        //    if (input.Length > 10)
        //    {
        //        if (!EsEnteroValido10Digitos(input))
        //        {
        //            MessageBox.Show("Por favor ingrese un número de teléfono válido de 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            textbox.Clear();
        //        }
        //    }
        //    else if (!EsEnteroValido10Digitos(input))
        //    {
        //        MessageBox.Show("Por favor ingrese un número de teléfono válido de 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!EsTextoValido(textbox.Text))
            {
                MessageBox.Show("Por favor ingrese un nombre válido (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textbox.Clear();
            }
        }

        private void ValidarApellido(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (!EsTextoValido(textbox.Text))
            {
                MessageBox.Show("Por favor ingrese apellidos válidos (solo letras y espacios).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textbox.Clear();
            }
        }

    }
}