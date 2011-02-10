﻿/*
 * Copyright 2010-2011 Bastian Eicher
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using NDesk.Options;
using ZeroInstall.Commands.Properties;
using ZeroInstall.Injector;
using ZeroInstall.Injector.Solver;
using ZeroInstall.Model;

namespace ZeroInstall.Commands
{
    /// <summary>
    /// Select a version of the program identified by URI, and compatible versions of all of its dependencies.
    /// </summary>
    [CLSCompliant(false)]
    public class Selection : CommandBase
    {
        #region Variables
        /// <summary>Cached <see cref="ISolver"/> results.</summary>
        protected Selections Selections;
        #endregion

        #region Properties
        /// <inheritdoc/>
        public override string Name { get { return "select"; } }

        /// <inheritdoc/>
        public override string Description { get { return "Select a version of the program identified by URI, and compatible versions of all of its dependencies. Returns an exit status of zero if it selected a set of versions, and a status of 1 if it could not find a consistent set."; } }

        /// <inheritdoc/>
        protected override string Usage { get { return "[OPTIONS] URI"; } }

        private readonly Requirements _requirements = new Requirements();
        /// <summary>
        /// A set of requirements/restrictions imposed by the user on the implementation selection process as parsed from the command-line arguments.
        /// </summary>
        public Requirements Requirements { get { return _requirements; } }
        #endregion

        #region Constructor
        /// <inheritdoc/>
        public Selection(IHandler handler) : base(handler)
        {
            Options.Add("batch", Resources.OptionBatch, unused => handler.Batch = true);
            Options.Add("r|refresh", Resources.OptionRefresh, unused => Policy.FeedManager.Refresh = true);
            
            Options.Add("command=", Resources.OptionCommand, command => _requirements.CommandName = command);
            Options.Add("before=", Resources.OptionBefore, version => _requirements.BeforeVersion = new ImplementationVersion(version));
            Options.Add("not-before=", Resources.OptionNotBefore, version => _requirements.NotBeforeVersion = new ImplementationVersion(version));
            Options.Add("s|source", Resources.OptionSource, unused => _requirements.Architecture = new Architecture(_requirements.Architecture.OS, Cpu.Source));
            Options.Add("os=", Resources.OptionOS, os => _requirements.Architecture = new Architecture(Architecture.ParseOS(os), _requirements.Architecture.Cpu));
            Options.Add("cpu=", Resources.OptionCpu, cpu => _requirements.Architecture = new Architecture(_requirements.Architecture.OS, Architecture.ParseCpu(cpu)));

            // ToDo: Add --xml

            // Work-around to disable interspersed arguments (needed for passing arguments through to sub-processes)
            Options.Add("<>", value =>
            {
                if (string.IsNullOrEmpty(Requirements.InterfaceID))
                { // First unknown argument
                    // Must not be an option
                    // Note: Windows-style arguments beginning with a slash are interpreted as Unix paths here instead
                    if (value.StartsWith("-")) throw new OptionException(Resources.UnknownOption, Name);

                    Requirements.InterfaceID = (File.Exists(value) ? Path.GetFullPath(value) : value);

                    // Stop using options parser, treat everything from here on as unknown
                    Options.Clear();
                }
                else
                { // All other unknown arguments
                    AdditionalArgs.Add(value);
                }
            });
        }
        #endregion

        //--------------------//

        #region Execute
        /// <inheritdoc/>
        protected override void ExecuteHelper()
        {
            base.ExecuteHelper();

            if (Requirements.InterfaceID == null) throw new InvalidInterfaceIDException(Resources.NoInterfaceSpecified);

            // ToDo: Detect Selections documents

            Selections = SolverProvider.Default.Solve(_requirements, Policy, Handler);
        }
        
        /// <inheritdoc/>
        public override int Execute()
        {
            if (AdditionalArgs.Count != 0) throw new OptionException(Resources.TooManyArguments, Name);

            ExecuteHelper();

            // ToDo: Output selections

            return 0;
        }
        #endregion
    }
}
