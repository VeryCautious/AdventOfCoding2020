Namespace Day8

    Public Class HandHeldConsoleSimulator

        Private ReadOnly SourceCode As Instruction()
        Private Property AccValue As Integer = 0

        Private Property InstructionPointer As Integer = 0
        Private Property LastInstruction As Instruction

        Public ReadOnly Property AccumulatorValue As Integer
            Get
                Return AccValue
            End Get
        End Property

        Public Sub New(SourceCode As String())
            Me.SourceCode = SourceCode.ZipWithIndex().ToArray.Map(AddressOf CreateInstructionFromLineOfCode)
        End Sub

        Public Sub StartSimulation()
            While CurrentInstruction IsNot Nothing AndAlso Not CurrentInstruction.WasExecuted
                CurrentInstruction.RunCommand()
            End While
        End Sub

        Public ReadOnly Property Terminated As Boolean
            Get
                Return InstructionPointer >= SourceCode.Count
            End Get
        End Property

        Private ReadOnly Property CurrentInstruction As Instruction
            Get
                Return If(Terminated, Nothing, SourceCode(InstructionPointer))
            End Get
        End Property

        Private Function CreateInstructionFromLineOfCode(IV As IndexValuePair(Of String)) As Instruction
            Dim Line = IV.Index
            Dim LineOfCode = IV.Value

            Select Case True
                Case LineOfCode.StartsWith("nop")
                    Return New NopInstruction(Me, Line)
                Case LineOfCode.StartsWith("acc")
                    Return New AccInstruction(LineOfCode, Me, Line)
                Case LineOfCode.StartsWith("jmp")
                    Return New JmpInstruction(LineOfCode, Me, Line)
                Case Else
                    Return New UnknownCommandInstruction(LineOfCode, Me, Line)
            End Select
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("[Acc: {0}, IP: {1}, (NextCmd: {2})]", AccumulatorValue, InstructionPointer, SourceCode(InstructionPointer).ToString)
        End Function

        Private Class Instruction
            Public Property WasExecuted As Boolean = False
            Public ReadOnly Property Line As Integer
            Protected Property Console As HandHeldConsoleSimulator
            Public Sub New(Console As HandHeldConsoleSimulator, Line As Integer)
                Me.Console = Console
                Me.Line = Line
            End Sub
            Public Overridable Sub RunCommand()
                WasExecuted = True
                Console.LastInstruction = Me
                Console.InstructionPointer += 1
            End Sub
        End Class

        Private Class NopInstruction
            Inherits Instruction
            Public Sub New(Console As HandHeldConsoleSimulator, Line As Integer)
                MyBase.New(Console, Line)
            End Sub
            Public Overrides Function ToString() As String
                Return "Nop"
            End Function
        End Class

        Private Class UnknownCommandInstruction
            Inherits Instruction
            Private ReadOnly LineOfCode As String
            Public Sub New(LineOfCode As String, Console As HandHeldConsoleSimulator, Line As Integer)
                MyBase.New(Console, Line)
                Me.LineOfCode = LineOfCode
            End Sub
            Public Overrides Sub RunCommand()
                Throw New ArgumentException("Command unknown: " + LineOfCode)
                MyBase.RunCommand()
            End Sub
            Public Overrides Function ToString() As String
                Return "UnknownCommand " + LineOfCode
            End Function
        End Class

        Private Class AccInstruction
            Inherits Instruction
            Private ReadOnly Property AccAmmount As Integer

            Public Sub New(LineOfCode As String, Console As HandHeldConsoleSimulator, Line As Integer)
                MyBase.New(Console, Line)
                AccAmmount = CInt(LineOfCode.Match("acc {0}")(0))
            End Sub
            Public Overrides Sub RunCommand()
                Console.AccValue += AccAmmount
                MyBase.RunCommand()
            End Sub
            Public Overrides Function ToString() As String
                Return "Acc " + AccAmmount.ToString
            End Function
        End Class

        Private Class JmpInstruction
            Inherits Instruction
            Private ReadOnly Property JmpAmmount As Integer

            Public Sub New(LineOfCode As String, Console As HandHeldConsoleSimulator, Line As Integer)
                MyBase.New(Console, Line)
                JmpAmmount = CInt(LineOfCode.Match("jmp {0}")(0))
            End Sub
            Public Overrides Sub RunCommand()
                Console.InstructionPointer += JmpAmmount
                Debug.Assert(Console.InstructionPointer >= 0)
                Console.InstructionPointer -= 1 ''Because it is always incremented by 1
                MyBase.RunCommand()
            End Sub
            Public Overrides Function ToString() As String
                Return "Jmp " + JmpAmmount.ToString
            End Function
        End Class

    End Class



End Namespace