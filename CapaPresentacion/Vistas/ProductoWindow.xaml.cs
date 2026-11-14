using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion.Vistas
{
    public partial class ProductoWindow : Window
    {
        private readonly ProductoBL _bl = new ProductoBL();

        public ProductoWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Listar();
            txtNombre.Focus();
        }

        private void Listar()
        {
            try
            {
                List<Producto> lista = _bl.Buscar(txtBuscar.Text);
                dgProductos.ItemsSource = lista;
                lblTotal.Text = "Total: " + lista.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Producto p = new Producto();
            p.Nombre = txtNombre.Text.Trim();
            p.Categoria = (cboCategoria.SelectedItem as ComboBoxItem)?.Content.ToString();
            decimal.TryParse(txtPrecio.Text, out decimal precio);
            p.Precio = precio;
            p.Stock = (int)sldStock.Value;
            p.Activo = chkActivo.IsChecked == true;

            try
            {
                string error = _bl.Registrar(p);
                if (error != "")
                {
                    MessageBox.Show(error, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Producto guardado", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                Limpiar();
                Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
                Listar();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtNombre.Clear();
            cboCategoria.SelectedIndex = -1;
            txtPrecio.Clear();
            sldStock.Value = 0;
            lblStockValor.Text = "0";
            chkActivo.IsChecked = true;
            dgProductos.SelectedItem = null;
            txtNombre.Focus();
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto p)
            {
                txtNombre.Text = p.Nombre;
                txtPrecio.Text = p.Precio.ToString("0.00");
                sldStock.Value = p.Stock;
                lblStockValor.Text = p.Stock.ToString();
                chkActivo.IsChecked = p.Activo;
                foreach (ComboBoxItem item in cboCategoria.Items)
                {
                    if (item.Content.ToString() == p.Categoria)
                        cboCategoria.SelectedItem = item;
                }
            }
        }

        private void SldStock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblStockValor != null)
            {
                lblStockValor.Text = ((int)sldStock.Value).ToString();
            }
        }

        private void SoloDecimales(object sender, TextCompositionEventArgs e)
        {
            bool esNumero = char.IsDigit(e.Text, 0);
            bool esPunto = e.Text == "." && !txtPrecio.Text.Contains(".");
            e.Handled = !(esNumero || esPunto);
        }
    }
}