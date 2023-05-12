using System;

namespace Entities.Models.Configuracion
{
    public class Clientes
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string RFC { get; set; }
        public string CalleNumero { get; set; }
        public string Colonia { get; set; }
        public string actCod_COMUNAColonia { get; set; }
        public string CP { get; set; }
        public string TelCasa { get; set; }
        public string TelOfc { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public string DelMun { get; set; }
        public string IDRef { get; set; }
        public string TipoCliente { get; set; }
        public string CardCode { get; set; }
        public string actFCHNACIMIENTOTemp { get; set; }
        public DateTime? actFCHNACIMIENTO { get; set; }
        public string actNOMBRE { get; set; }
        public string actAPEPATERNO { get; set; }
        public string actAPEMATERNO { get; set; }
        public string actCALLE { get; set; }
        public string actNUMEXT { get; set; }
        public string actNUMINT { get; set; }
        public string actCod_REGIONestado { get; set; }
        public string actCod_PROVINCIAmunicipio { get; set; }
        public string actCod_CIUDAD { get; set; }
        public string actCIUDAD { get; set; }
        public string Referencia { get; set; }
        public string FormaPago33 { get; set; }
        public string MetodoPago33 { get; set; }
        public string TipoComp33 { get; set; }
        public string UsoCFDI33 { get; set; }
        public string TipoRel33 { get; set; }
        public string CFDI_Rel33 { get; set; }
        public string NoCelular { get; set; }

        public string DelMunFact { get; set; }
        public string ColoniaFact { get; set; }
        public string actCod_COMUNAColoniaFact { get; set; }
        public string CalleNumeroFact { get; set; }
        public string actNUMEXTFact { get; set; }
        public string actNUMINTFact { get; set; }
        public string actCod_REGIONestadoFact { get; set; }
        public string CPFact { get; set; }
        public int IdStore { get; set; }
        public string Domicilio { get; set; }
        public string DomicilioFact { get; set; }
        public int? RegimenFiscal { get; set; }
    }
}
