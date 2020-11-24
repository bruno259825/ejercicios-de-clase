'Enumeracion de estados
Enum Estado
    LIBRE       '0
    CABALLO     '1
    AMENAZADA   '2
End Enum
'Estructura coordenada
Structure Coordenada
    Dim x, y As Integer
End Structure

'Estructura de las casillas del tablero
Structure Casilla
    'Estado de la casilla
    Dim enEst As Estado
    'Array de coordenadas de caballos que amenazan una casilla
    Dim eCoord() As Coordenada
End Structure

Module Caballo

    Sub Main()

        'Declaracion de variables
        Dim eTablero(7, 7) As Casilla       'Tablero de casillas
        Dim iCaballosColocados As Integer   'Numero de caballos colocados
        Dim eCoord As Coordenada            'Coordenada a leer
        Dim bCaballoColocado As Boolean     'Indicativo de caballo colocado

        Const NUM_MAXIMO_CABALLOS As Integer = 4 'Numero de caballos a colocar


        'Inicializamos el tablero
        InicializarTablero(eTablero)

        iCaballosColocados = 0
        Do

            'Presentamos el estado del tablero por pantalla
            PresentarTablero(eTablero)

            'Leemos coordenadas validas
            eCoord = LeerCoordenadas()

            'Colocamos (o intentamos) colocar el caballo en las coordenadas leidas
            bCaballoColocado = ColocarCaballo(eTablero, eCoord)

            If bCaballoColocado = True Then
                'El caballo se ha colocado, incrementamos el contador
                iCaballosColocados = iCaballosColocados + 1
            End If

        Loop While iCaballosColocados < NUM_MAXIMO_CABALLOS

        'Presentamos el tablero con el estado final
        PresentarTablero(eTablero)

        Console.WriteLine(" ESTADO FINAL DEL TABLERO")

        'Lectura para pausa
        Console.ReadLine()

    End Sub

    Sub InicializarTablero(ByVal eTablero(,) As Casilla)
        'Variables locales
        Dim i, j As Integer

        'Para cada casilla del tablero
        For i = 0 To eTablero.GetUpperBound(0)
            For j = 0 To eTablero.GetUpperBound(1)
                'Ponemos cada casilla a libre
                eTablero(i, j).enEst = Estado.LIBRE

                'Inicializamos el array que almacena las coordenadas de los caballos
                'amenazantes de esta posicion
                ReDim eTablero(i, j).eCoord(-1)

            Next
        Next

    End Sub

    Sub PresentarTablero(ByVal eTablero(,) As Casilla)
        'Variables locales
        Dim i, j As Integer
        Dim cEstado, cFila As Char 'Para la conversion a caracter de estado y fila

        'Limpiamos la pantalla
        LimpiarPantalla()

        Console.WriteLine("       1 2 3 4 5 6 7 8")
        Console.WriteLine("       ---------------")

        For i = 0 To eTablero.GetUpperBound(0)
            cFila = Letra(i)
            Console.Write("   " & cFila & " | ")
            For j = 0 To eTablero.GetUpperBound(1)
                cEstado = TraduceEstado(eTablero(i, j).enEst)

                Console.Write(cEstado & " ")
            Next
            Console.Write("|")
            Console.WriteLine()
            Console.WriteLine("       ---------------")
        Next

    End Sub
    Function LeerCoordenadas() As Coordenada
        'Variables
        Dim eCoord As Coordenada 'Para almacenar la coordenada a devolver
        Dim sLeer As String     'Para almacenar la entrada
        Dim cLeer As Char       'Para almacenar el caracter leido
        Dim bValido As Boolean  'Para comprobar la validez de una entrada
        Dim x, y As Integer

        'Esta funcion lee unas coordenadas validas
        'Leer fila
        bValido = False
        Do

            Console.WriteLine("Introduce una fila valida (A-H)")
            sLeer = Console.ReadLine()

            'Compruebo la longitud de la cadena leida
            If sLeer.Length = 1 Then
                'Paso a caracter
                cLeer = Convert.ToChar(sLeer)

                'Compruebo el tipo de caracter leido
                If Char.IsLetter(cLeer) = True Then
                    'Paso a mayusculas
                    cLeer = Char.ToUpper(cLeer)

                    'Si el caracter esta dentro del intervalo valido
                    If cLeer >= "A"c And cLeer <= "H"c Then
                        'Hago la conversion a digito
                        x = Digito(cLeer)
                        'Entrada valida
                        bValido = True
                    End If
                End If
            End If
        Loop While bValido = False

        'Leer columna
        bValido = False
        Do
            Console.WriteLine("Introduce una columna valida (1-8)")
            sLeer = Console.ReadLine()

            'Compruebo la longitud de la cadena leida
            If sLeer.Length = 1 Then

                'Compruebo el tipo lo leido
                If IsNumeric(sLeer) = True Then
                    'Hago la conversion implicita a digito y resto 1
                    y = sLeer
                    y = y - 1

                    'Compruebo el valor obtenido
                    If y >= 0 And y <= 7 Then
                        bValido = True
                    End If
                End If
            End If
        Loop While bValido = False

        'Paso a estructura Coordenada
        eCoord.x = x
        eCoord.y = y

        Return eCoord
    End Function

    Function ColocarCaballo(ByVal eTablero(,) As Casilla, ByVal eCoord As Coordenada) As Boolean
        Dim bCaballoColocado As Boolean

        'Recibe el tablero y unas coordenadas donde colocar el caballo
        'Verifica el tablero e intenta colocar el caballo actualizando el tablero

        bCaballoColocado = False
        'Segun el estado de la casilla podremos colocar o no
        Select Case eTablero(eCoord.x, eCoord.y).enEst
            Case Estado.LIBRE
                'Colocar caballo
                eTablero(eCoord.x, eCoord.y).enEst = Estado.CABALLO

                'Marcar amenazadas
                MarcarAmenazadas(eTablero, eCoord)

                'Lo he colocado
                bCaballoColocado = True

            Case Estado.CABALLO
                Console.WriteLine("YA EXISTE UN CABALLO EN LA COORDENADA SOLICITADA")
                Console.ReadLine()

                'No lo he colocado
                bCaballoColocado = False

            Case Estado.AMENAZADA
                Console.WriteLine("LA COORDENADA SOLICITADA ESTA AMENAZADA POR AL MENOS UN CABALLO")
                'Realiza un listado de las coordenadas que amenazan esta
                ListarCaballosAmenazantes(eTablero, eCoord)
                Console.ReadLine()

                'No lo he colocado
                bCaballoColocado = False

        End Select

        Return bCaballoColocado
    End Function
    Sub MarcarAmenazadas(ByVal eTablero(,) As Casilla, ByVal eCoord As Coordenada)

        'Defino arrays de movimientos
        Dim iHor() As Integer = {-2, -1, +1, +2, +2, +1, -1, -2}
        Dim iVert() As Integer = {+1, +2, +2, +1, -1, -2, -2, -1}
        Dim i As Integer
        Dim x, y As Integer
        Dim eCoordAmenazada As Coordenada

        'Recorro los movimientos
        For i = 0 To iHor.GetUpperBound(0)

            'Genero coordenada amenazante
            x = eCoord.x + iHor(i)
            y = eCoord.y + iVert(i)
            eCoordAmenazada.x = x
            eCoordAmenazada.y = y

            'Pruebo su validez (0-7)
            If x >= 0 And x <= 7 And y >= 0 And y <= 7 Then
                'Marco la casilla en el tablero como amenazada
                eTablero(x, y).enEst = Estado.AMENAZADA

                'Indico qué caballo amenaza guardando su coordenada en el array de coordenadas de caballos amenazantes
                ReDim Preserve eTablero(x, y).eCoord(eTablero(x, y).eCoord.GetUpperBound(0) + 1)

                eTablero(x, y).eCoord(eTablero(x, y).eCoord.GetUpperBound(0)) = eCoord

            End If
        Next

    End Sub
    Sub ListarCaballosAmenazantes(ByVal eTablero(,) As Casilla, ByVal eCoord As Coordenada)
        'Variables locales
        Dim i As Integer
        Dim eCoordAux As Coordenada

        Console.Write("Caballos amenazantes en: ")

        'Recorro el array de coordenadas de caballos amenazantes
        For i = 0 To eTablero(eCoord.x, eCoord.y).eCoord.GetUpperBound(0)
            'Almaceno coordenada
            eCoordAux = eTablero(eCoord.x, eCoord.y).eCoord(i)

            Console.Write(Letra(eCoordAux.x))
            Console.Write("-")
            Console.Write(eCoordAux.y + 1) 'Hay que sumar 1 : 0-7 -> 1-8
            Console.Write(" ")

        Next

    End Sub
    Function Letra(ByVal i As Integer) As Char
        'Variables locales
        Dim cLetra As Char

        'Devuelvo el valor Alfabetico de una coordenada numerica
        Select Case i
            Case 0
                cLetra = "A"c
            Case 1
                cLetra = "B"c
            Case 2
                cLetra = "C"c
            Case 3
                cLetra = "D"c
            Case 4
                cLetra = "E"c
            Case 5
                cLetra = "F"c
            Case 6
                cLetra = "G"c
            Case 7
                cLetra = "H"c
        End Select

        Return cLetra
    End Function
    Function Digito(ByVal c As Char) As Integer
        'Variables locales
        Dim i As Integer

        'Devuelvo el valor numerico de una coordenada alfabetica
        Select Case c
            Case "A"c
                i = 0
            Case "B"c
                i = 1
            Case "C"c
                i = 2
            Case "D"c
                i = 3
            Case "E"c
                i = 4
            Case "F"c
                i = 5
            Case "G"c
                i = 6
            Case "H"c
                i = 7
        End Select

        Return i
    End Function
    Function TraduceEstado(ByVal eEstado As Estado) As Char
        'Variables locales
        Dim cEstado As Char

        'Devuelvo el caracter que representa el estado de una casilla
        Select Case eEstado
            Case Ajedrez.Estado.LIBRE
                cEstado = "·"c
            Case Ajedrez.Estado.AMENAZADA
                cEstado = "X"c
            Case Ajedrez.Estado.CABALLO
                cEstado = "C"c
        End Select

        Return cEstado
    End Function
    Sub LimpiarPantalla()
        'Variables
        Dim i As Integer

        'Bucle limpiador
        For i = 0 To 60
            Console.WriteLine("")
        Next

    End Sub
End Module
