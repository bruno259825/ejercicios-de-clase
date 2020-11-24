"use strict";
// Clase Producto
function Producto(iCodigo, sNombre, dbPrecio, iUnidades){
    this.codigo = iCodigo;
    this.nombre = sNombre;
    this.precio = dbPrecio;
    this.unidades = iUnidades;
}


Producto.prototype.toHTMLRow = function (){
    let sFila = "<tr>";
    sFila += "<td>" + this.codigo + "</td>";
    sFila += "<td>" + this.nombre + "</td>";
    sFila += "<td>" + this.precio + "</td>";
    sFila += "<td>" + this.unidades + "</td>";
    sFila += "</tr>";

    return sFila;
}

// Clase Almacén
class Almacen{
    constructor(){
        this.productos = [];
    }

    altaRemesa (oProducto){
        let oProductoExistente = null;

        oProductoExistente = this._buscarProducto(oProducto.codigo);

        // Si el producto no existe lo inserto
        if(oProductoExistente == null){
            this.productos.push(oProducto);
        } else {
            oProductoExistente.unidades += oProducto.unidades;
            oProductoExistente.precio = oProducto.precio;
        }
    }

    _buscarProducto(iCodigo){
       let oProductoExistente = null;
       //oProductoExistente =  this.productos.find(function(oProducto){ 
       //     return oProducto.codigo == iCodigo;});
       oProductoExistente =  this.productos.find(oProducto => oProducto.codigo == iCodigo);

        return oProductoExistente;
    }

    _eliminarProducto(oProducto){
        let iPos=this.productos.findIndex(oPro=>oPro.codigo==oProducto.codigo);
        
        this.productos.splice(iPos,1);
    }

    // Devuelve una tabla HTML con el listado
    // de productos ordenados por código de producto
    productosPorCodigo(){

        let sTabla = '<table border="1">';

        // Encabezado de la tabla
        sTabla += "<thead><tr>";
        sTabla += "<th>Código</th><th>Nombre</th>";
        sTabla += "<th>Precio</th><th>Unidades</th>";
        sTabla += "</tr></thead>";

        // Contenido de la tabla
        sTabla += "<tbody>";

        // Obtenemos array que no tiene productos con 0 unidades
        let oProductosAux  = this.productos.filter( oProducto => oProducto.unidades > 0 );

        // Obtenemos array ordenado por código de producto
        oProductosAux.sort(function (oP1,oP2){return oP1.codigo - oP2.codigo;});

        for (let oP of oProductosAux){
            sTabla += oP.toHTMLRow();
        }

        sTabla += "</tbody>";

        return sTabla;
    }

    productosPorValor(){
        let sTabla = '<table border="1">';

        // Encabezado de la tabla
        sTabla += "<thead><tr>";
        sTabla += "<th>Código</th><th>Nombre</th>";
        sTabla += "<th>Precio</th><th>Unidades</th>";
        sTabla += "</tr></thead>";

        // Contenido de la tabla
        sTabla += "<tbody>";

        // Obtenemos array que no tiene productos con 0 unidades
        let oProductosAux  = this.productos.filter( oProducto => oProducto.unidades > 0 );

        // Obtenemos array ordenado por código de producto
        oProductosAux.sort(function (oP1,oP2){return oP2.precio*oP2.unidades-oP1.precio*oP1.unidades;});

        for (let oP of oProductosAux){
            sTabla += oP.toHTMLRow();
        }

        sTabla += "</tbody>";

        return sTabla;
    }

    listadoProductos(){
        let sTabla = '<table border="1">';

        // Encabezado de la tabla
        sTabla += "<thead><tr>";
        sTabla += "<th>Código</th><th>Nombre</th>";
        sTabla += "<th>Precio</th><th>Unidades</th>";
        sTabla += "</tr></thead>";

        // Contenido de la tabla
        sTabla += "<tbody>";

        // Obtenemos array que no tiene productos con 0 unidades
        let oProductosAux  = this.productos.filter( oProducto => oProducto.unidades > 0 );

        
        for (let oP of oProductosAux){
            sTabla += oP.toHTMLRow();
        }

        sTabla += "</tbody>";

        return sTabla;
    }

}
