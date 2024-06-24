using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificado.Dominio.Contrato
{
    public interface InterfaceBase<T> where T : class
    {
        bool Salvar(T entidade);
        string Excluir(T entidade);
        T ListarPorId(string id);
        IEnumerable<T> ListarTodos();
    }
}
