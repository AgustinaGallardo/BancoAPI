using CRUDbanco.Datos;
using CRUDbanco.Servicios.Implementacion;
using CRUDbanco.Servicios.Interfaz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDbanco
{
    public partial class AltaCliente : Form
    {
        private IServicio gestor;
        private Clientes nuevo;
        public AltaCliente()
        {
            InitializeComponent();
            gestor = new Servicio();
            nuevo= new Clientes();
        }

        private void AltaCliente_Load(object sender, EventArgs e)
        {
            ObtenerProximo();
            CargarCombo();
        }

        private void CargarCombo()
        {
            cboTipoCuenta.ValueMember="Id";
            cboTipoCuenta.DisplayMember="Tipo";
            cboTipoCuenta.DataSource  = gestor.ObtenerCuentas();
            cboTipoCuenta.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void ObtenerProximo()
        {
            int Next = gestor.ObtenerProximo();

            if(Next > 0)
            {
                lblProximoCliente.Text = "Cliente Nro: " + Next.ToString();
            }
            else
            {
                MessageBox.Show("No se puede obtener el proximo cliente", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(txtSaldo.Text == "")
            {
                MessageBox.Show("Cargar un Saldo", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
            if(txtUltimoMov.Text == "")
            {
                MessageBox.Show("Cargue el ultimo movimiento", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
            if(txtcbu.Text == "")
            {
                MessageBox.Show("Cargar el cbu", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
            foreach(DataGridViewRow item in dgvClientes.Rows)
            {
                if(item.Cells["colTipoCuenta"].Value.ToString().Equals(cboTipoCuenta.Text))
                {
                    MessageBox.Show("El tipo Cuenta: " + cboTipoCuenta.Text + " Ya esta cargado", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
                    return;
                }
               
            }

            TipoCuenta tp = (TipoCuenta)cboTipoCuenta.SelectedItem;

            double saldo = Convert.ToDouble (txtSaldo.Text);
            int cbu = Convert.ToInt32(txtcbu.Text);
            DateTime fecha = Convert.ToDateTime(txtUltimoMov.Text);

            Cuenta cuenta = new Cuenta(cbu,saldo,fecha,tp);
            nuevo.AgregarCuenta(cuenta);

            dgvClientes.Rows.Add(cuenta.Cbu, cuenta.tipoCuenta.Tipo, cuenta.Saldo, cuenta.UltimoMovimiento);

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtApellido.Text == "")
            {
                MessageBox.Show("Cargar el cbu", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }


            nuevo.Apellido = txtApellido.Text;
            nuevo.Nombre = txtNombre.Text;
            nuevo.Dni = Convert.ToInt32(txtDni.Text);

            if (Helper.ObtenerInstancia().ConfirmarCliente(nuevo))
            {
                MessageBox.Show("Se inserto con exito", "BIEN", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show("NO inserto con exito", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
            }
         
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvClientes.CurrentCell.ColumnIndex == 4)
            {
                nuevo.EliminarCuenta(dgvClientes.CurrentRow.Index);
                dgvClientes.Rows.Remove(dgvClientes.CurrentRow);
                
            }
        }
    }
}
