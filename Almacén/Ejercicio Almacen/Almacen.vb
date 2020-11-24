'Estructura para los productos
Public Structure Producto
    Public iCodProducto As Integer
    Public sNombre As String
    Public dbPrecio As Double
End Structure
'Estructura StockProducto para el almacen
Public Structure StockProducto
    Public iCodProducto As Integer
    Public iNumProductos As Integer
End Structure
'Enumeracion de opciones del menu principal
Public Enum OpcionesPpal
    ALTA_REMESA = 1 '1
    SALIDA_REMESA   '2
    ALTA_PRODUCTO   '3
    LISTADOS        '4
    SALIR           '5
End Enum
'Enumeracion de opciones del menu de listados
Public Enum OpcionesListado
    LISTADO_BASICO = 1 '1
    LISTADO_CATALOGO   '2
    LISTADO_CODIGO     '3
    SALIR              '4
End Enum

Module Almacen

    'Procedimiento principal
    Sub Main()
        'Declaración de variables
        Dim esAlmacen(-1) As StockProducto
        Dim esCatalogo(-1) As Producto
        Dim enOpcion As OpcionesPpal

        'Bucle principal del menu
        Do
            'Presentar menu de opciones
            enOpcion = Menu()

            Select Case enOpcion
                Case OpcionesPpal.ALTA_REMESA

                    AltaRemesa(esAlmacen, esCatalogo)

                Case OpcionesPpal.SALIDA_REMESA

                    SalidaRemesa(esAlmacen, esCatalogo)

                Case OpcionesPpal.ALTA_PRODUCTO

                    AltaProducto(esCatalogo)

                Case OpcionesPpal.LISTADOS

                    Listados(esAlmacen, esCatalogo)
            End Select

        Loop While enOpcion <> OpcionesPpal.SALIR

    End Sub

    Public Function Menu() As OpcionesPpal
        'Declaracion de variables
        Dim enOpcion As OpcionesPpal
        Dim iNum As Integer

        Do
            'Limpiar pantalla
            Console.Clear()

            'Mostramos menu
            Console.WriteLine("APLICACION DE ALMACEN")
            Console.WriteLine("")
            Console.WriteLine("1.ALTA DE REMESA")
            Console.WriteLine("2.SALIDA DE REMESA")
            Console.WriteLine("3.ALTA DE PRODUCTO")
            Console.WriteLine("4.LISTADOS")
            Console.WriteLine("5.SALIR")
            Console.WriteLine("")
            iNum = LeerNumerico("Introduzca una opcion valida")

        Loop While iNum < 1 And iNum > 4

        'Ya es valida la entrada, convertimos a la enumeracion OpcionesPpal
        enOpcion = iNum

        Return enOpcion

    End Function
    Function LeerNumerico(ByVal sMensaje As String) As Double
        Dim sEntrada As String

        Do
            'Mostramos el mensaje
            Console.WriteLine(sMensaje)
            'Leemos la entrada
            sEntrada = Console.ReadLine()

        Loop While Not IsNumeric(sEntrada)

        Return sEntrada 'Conversion implicita al devolver

    End Function
    Public Sub AltaRemesa(ByRef esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim esRemesa As StockProducto

        'Tomamos los datos de la remesa de entrada
        esRemesa = LeerRemesa()

        'Verificamos si el producto está en el catálogo
        If ProductoExiste(esCatalogo, esRemesa.iCodProducto) Then
            'Registramos el producto en el almacen
            EntradaAlmacen(esAlmacen, esRemesa)

        Else 'En otro caso informamos del problema
            Console.WriteLine("El producto no está en el catálogo")
            Console.WriteLine("Delo de alta antes de recepcionar la remesa")
            Console.ReadLine()
        End If
        
    End Sub
    Public Sub SalidaRemesa(ByRef esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim esRemesa As StockProducto

        'Tomamos los datos de la remesa de salida
        esRemesa = LeerRemesa()

        If ProductoExiste(esCatalogo, esRemesa.iCodProducto) Then

            'Actualizamos en almacen
            SalidaAlmacen(esAlmacen, esRemesa)

        Else 'En otro caso informamos del problema
            Console.WriteLine("El producto no está en el catálogo y por tanto tampoco en almacen")
            Console.ReadLine()
        End If

    End Sub
    Public Sub AltaProducto(ByRef esCatalogo() As Producto)
        'Variables locales
        Dim esProducto As Producto

        esProducto = LeerProducto

        If ProductoExiste(esCatalogo, esProducto.iCodProducto) Then
            Console.WriteLine("El producto ya existe en el catálogo, no se dará de alta")
        Else

            'Insertamos el producto en el catálogo
            'Primero : redimensionamos el array
            ReDim Preserve esCatalogo(esCatalogo.GetUpperBound(0) + 1)
            'Segundo : añadimos al final
            esCatalogo(esCatalogo.GetUpperBound(0)) = esProducto

            Console.WriteLine("Producto añadido al catálogo")
        End If

        Console.ReadLine()

    End Sub
    Public Sub Listados(ByRef esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim enOpcionListado As OpcionesListado

        'Bucle principal del menu de listados
        Do
            'Presentar menu de opciones
            enOpcionListado = MenuListado()

            Select Case enOpcionListado
                Case OpcionesListado.LISTADO_BASICO

                    ListadoBasico(esAlmacen, esCatalogo)

                Case OpcionesListado.LISTADO_CATALOGO

                    ListadoCatalogo(esCatalogo)

                Case OpcionesListado.LISTADO_CODIGO

                    ListadoOrdenado(esAlmacen, esCatalogo)

            End Select
        Loop While enOpcionListado <> OpcionesListado.SALIR
    End Sub
    Public Function LeerRemesa() As StockProducto
        'Declaracion de variables
        Dim esRemesa As StockProducto

        'Leemos los datos de la remesa
        'Primero el codigo de producto
        esRemesa.iCodProducto = LeerNumerico("Introduzca el código del producto")

        'Después el numero de productos
        esRemesa.iNumProductos = LeerNumerico("Introduzca el numero de productos")

        'Devolvemos la remesa
        Return esRemesa

    End Function
    Public Function LeerProducto() As Producto
        'Declaracion de variables
        Dim esProducto As Producto

        'Leemos los datos del producto
        'Primero el codigo de producto
        esProducto.iCodProducto = LeerNumerico("Introduzca el código del producto")

        'Segundo el nombre del producto
        Console.WriteLine("Introduzca el nombre del producto")
        esProducto.sNombre = Console.ReadLine

        'Después el precio del producto
        esProducto.dbPrecio = LeerNumerico("Introduzca el precio del producto")

        'Devolvemos la remesa
        Return esProducto

    End Function

    Public Sub EntradaAlmacen(ByRef esAlmacen() As StockProducto, ByVal esRemesa As StockProducto)
        'Declaracion de variables
        Dim iPosicion As Integer

        iPosicion = BuscarProducto(esAlmacen, esRemesa.iCodProducto)

        Select Case iPosicion
            Case -1 'Producto no encontrado en almacen, lo añadimos

                'Redimensionamos el array +1
                ReDim Preserve esAlmacen(esAlmacen.GetUpperBound(0) + 1)
                'Guardamos los datos en la ultima posicion
                esAlmacen(esAlmacen.GetUpperBound(0)).iCodProducto = esRemesa.iCodProducto
                esAlmacen(esAlmacen.GetUpperBound(0)).iNumProductos = esRemesa.iNumProductos

            Case Is >= 0 'Producto encontrado, sumamos existencias

                esAlmacen(iPosicion).iNumProductos += esRemesa.iNumProductos

        End Select

        Console.WriteLine("Remesa procesada correctamente")
        Console.ReadLine()

    End Sub

    Public Sub SalidaAlmacen(ByRef esAlmacen() As StockProducto, ByVal esRemesa As StockProducto)
        'Declaracion de variables
        Dim iPosicion As Integer

        iPosicion = BuscarProducto(esAlmacen, esRemesa.iCodProducto)

        Select Case iPosicion
            Case -1 'Producto no encontrado, mostramos error

                Console.WriteLine("El codigo de producto no existe en el almacen")

            Case Is >= 0

                'Verificamos si hay existencias suficientes
                If esRemesa.iNumProductos < esAlmacen(iPosicion).iNumProductos Then

                    'En este caso solo hay que restar el numero de productos
                    esAlmacen(iPosicion).iNumProductos -= esRemesa.iNumProductos

                    Console.WriteLine("Remesa procesada correctamente")

                ElseIf esRemesa.iNumProductos = esAlmacen(iPosicion).iNumProductos Then

                    'En este caso hay que eliminar el producto del almacen de iPosicion
                    ' Primero : Almacenar el último elemento en la posicion iPosicion
                    esAlmacen(iPosicion) = esAlmacen(esAlmacen.GetUpperBound(0))

                    ' Segundo : Redimensionar el array a un elemento menos
                    ReDim Preserve esAlmacen(esAlmacen.GetUpperBound(0) - 1)

                    Console.WriteLine("Remesa procesada correctamente")

                Else
                    'Este es el caso de que no hay existencias suficientes
                    Console.WriteLine("No hay suficientes existencias del producto en el almacen")
                    Console.WriteLine("En el almacen hay solo " & esAlmacen(iPosicion).iNumProductos & " unidades del producto")

                End If

        End Select

        Console.ReadLine()
    End Sub
    Function ProductoExiste(ByVal esCatalogo() As Producto, ByVal iCodProducto As Integer) As Boolean
        'Declaración de variables
        Dim bExiste As Boolean

        'Recorremos el array en busca del producto
        bExiste = False
        For i As Integer = 0 To esCatalogo.GetUpperBound(0)
            If esCatalogo(i).iCodProducto = iCodProducto Then
                bExiste = True
                Exit For
            End If
        Next

        Return bExiste
    End Function
    Function BuscarProductoCatalogo(ByVal esCatalogo() As Producto, ByVal iCodProducto As Integer) As Producto
        'Declaracion de variables
        Dim esProducto As Producto = Nothing

        For Each esProd As Producto In esCatalogo
            If esProd.iCodProducto = iCodProducto Then
                esProducto = esProd
                Exit For
            End If
        Next

        Return esProducto
    End Function
    Function BuscarProducto(ByRef esAlmacen() As StockProducto, ByVal iCodProducto As Integer) As Integer
        'Declaración de variables
        Dim i, iPosicion As Integer

        'Recorremos el array en busca del producto
        iPosicion = -1
        For i = 0 To esAlmacen.GetUpperBound(0)
            If esAlmacen(i).iCodProducto = iCodProducto Then
                iPosicion = i
                Exit For
            End If
        Next

        Return iPosicion
    End Function
    Public Function MenuListado() As OpcionesListado
        'Declaracion de variables
        Dim enOpcion As OpcionesListado
        Dim iNum As Integer

        Do
            'Limpiar pantalla
            Console.Clear()

            'Mostramos menu
            Console.WriteLine("MENU DE LISTADOS")
            Console.WriteLine("")
            Console.WriteLine("1.LISTADO BASICO")
            Console.WriteLine("2.PRODUCTOS EN CATÁLOGO")
            Console.WriteLine("3.PRODUCTOS POR CÓDIGO")
            Console.WriteLine("4.SALIR")
            Console.WriteLine("")

            'Leemos la entrada
            iNum = LeerNumerico("Introduzca una opción válida")

        Loop While iNum < 1 And iNum > 4

        'Ya es valida la entrada, convertimos a la enumeracion OpcionesPpal
        enOpcion = iNum

        Return enOpcion

    End Function
    Sub ListadoBasico(ByVal esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Sacamos por pantalla la cabecera del listado
        CabeceraListado()

        'Sacamos las lineas de detalle
        CuerpoListado(esAlmacen, esCatalogo)

        'Lectura para poder ver el listado
        Console.ReadLine()
    End Sub
    Sub ListadoCatalogo(ByVal esCatalogo() As Producto)
        'Sacamos por pantalla la cabecera del listado
        CabeceraListadoCatalogo()

        'Sacamos las lineas de detalle
        CuerpoListadoCatalogo(esCatalogo)

        'Lectura para poder ver el listado
        Console.ReadLine()
    End Sub
    Sub ListadoOrdenado(ByVal esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim i As Integer
        Dim esAlmacenOrdenado() As StockProducto

        'Damos tamaño al array que quedará ordenado
        ReDim esAlmacenOrdenado(esAlmacen.GetUpperBound(0))

        'Copiamos los elementos
        Array.Copy(esAlmacen, esAlmacenOrdenado, esAlmacen.Length)

        'Llamamos a la función de ordenacion 
        OrdenaArray(esAlmacenOrdenado)

        'Sacamos por pantalla la cabecera del listado
        CabeceraListado()

        'Mostramos las lineas de detalle del listado
        CuerpoListado(esAlmacenOrdenado, esCatalogo)

        'Lectura para poder ver el listado
        Console.ReadLine()
    End Sub
    Sub CabeceraListado()
        Console.WriteLine()
        Console.Write("CODIGO PRODUCTO")
        Console.Write(ControlChars.Tab)
        Console.Write("NOMBRE PRODUCTO")
        Console.Write(ControlChars.Tab)
        Console.Write("PRECIO")
        Console.Write(ControlChars.Tab)
        Console.Write("CANTIDAD")
        Console.Write(ControlChars.Tab)
        Console.WriteLine("VALOR")
        Console.WriteLine("*************************************************************")
    End Sub
    Sub CabeceraListadoCatalogo()
        Console.WriteLine()
        Console.Write("CODIGO PRODUCTO")
        Console.Write(ControlChars.Tab)
        Console.Write("NOMBRE PRODUCTO")
        Console.Write(ControlChars.Tab)
        Console.WriteLine("PRECIO")
        Console.WriteLine("**********************************************************")
    End Sub
    Sub CuerpoListado(ByVal esAlmacen() As StockProducto, ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim i As Integer
        Dim esProducto As Producto

        'Recorremos el array y sacamos una linea por producto
        For i = 0 To esAlmacen.GetUpperBound(0)

            esProducto = BuscarProductoCatalogo(esCatalogo, esAlmacen(i).iCodProducto)

            Console.Write(esAlmacen(i).iCodProducto)
            Console.Write(ControlChars.Tab)
            Console.Write(ControlChars.Tab)

            Console.Write(esProducto.sNombre)
            Console.Write(ControlChars.Tab)
            Console.Write(ControlChars.Tab)

            Console.Write(esProducto.dbPrecio)
            Console.Write(ControlChars.Tab)

            Console.Write(esAlmacen(i).iNumProductos)
            Console.Write(ControlChars.Tab)
            Console.Write(ControlChars.Tab)

            Console.Write(esAlmacen(i).iNumProductos * esProducto.dbPrecio)
            Console.WriteLine("")
        Next

    End Sub
    Sub CuerpoListadoCatalogo(ByVal esCatalogo() As Producto)
        'Declaracion de variables
        Dim i As Integer

        'Recorremos el array y sacamos una linea por producto
        For i = 0 To esCatalogo.GetUpperBound(0)

            Console.Write(esCatalogo(i).iCodProducto)
            Console.Write(ControlChars.Tab)
            Console.Write(ControlChars.Tab)

            Console.Write(esCatalogo(i).sNombre)
            Console.Write(ControlChars.Tab)
            Console.Write(ControlChars.Tab)

            Console.Write(esCatalogo(i).dbPrecio)
            Console.WriteLine("")

        Next

    End Sub

    Sub OrdenaArray(ByVal esAlmacen() As StockProducto)
        'Declaracion de variables
        Dim i, j As Integer
        Dim bOrdenado, bCompara As Boolean
        Dim esElem As StockProducto

        'Usaremos el algoritmo de la burbuja mejorado
        'Inicializamos la marca que nos indicará cuando esta el array ordenado
        bOrdenado = False
        i = 0
        Do While bOrdenado = False And i < esAlmacen.GetUpperBound(0)
            bOrdenado = True

            For j = 0 To esAlmacen.GetUpperBound(0) - 1 - i
                'Comparacion por codigo de producto
                bCompara = esAlmacen(j).iCodProducto > esAlmacen(j + 1).iCodProducto
                If bCompara = True Then
                    'Intercambiamos elementos
                    esElem = esAlmacen(j)
                    esAlmacen(j) = esAlmacen(j + 1)
                    esAlmacen(j + 1) = esElem
                    'Marcamos como que el array aun no esta ordenado
                    bOrdenado = False
                End If
            Next
            'Incrementamos i
            i += 1
        Loop

    End Sub
End Module
