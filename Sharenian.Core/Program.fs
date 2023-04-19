module Sharenian.Program

open Serilog
open Serilog.Extensions.Logging
open Elmish.WPF

type GuildSewer = {
    Order: int
    Name: string
    Level: int
    Master: string
    Score: int }

let main window =
    let logger = 
        LoggerConfiguration()
            .MinimumLevel.Override("Elmish.WPF.Update", Events.LogEventLevel.Verbose)
            .MinimumLevel.Override("Elmish.WPF.Bindings", Events.LogEventLevel.Verbose)
            .MinimumLevel.Override("Elmish.WPF.Performance", Events.LogEventLevel.Verbose)
            .WriteTo.Console()
            .CreateLogger()

    WpfProgram.mkSimple init update bindings
    |> WpfProgram.withLogger (new SerilogLoggerFactory(logger))
    |> WpfProgram.startElmishLoop window