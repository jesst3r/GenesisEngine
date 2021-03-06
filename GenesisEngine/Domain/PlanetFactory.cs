﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StructureMap;

namespace GenesisEngine
{
    public interface IPlanetFactory
    {
        IPlanet Create(GenesisEngine.DoubleVector3 location, double radius);
    }

    public class PlanetFactory : IPlanetFactory
    {
        readonly ITerrainFactory _terrainFactory;
        readonly ISettings _settings;

        public PlanetFactory(ITerrainFactory terrainFactory, ISettings settings)
        {
            _terrainFactory = terrainFactory;
            _settings = settings;
        }

        public IPlanet Create(DoubleVector3 location, double radius)
        {
            // TODO: should we inject into the factory everything it will need to inject
            // into the planets?  That reduces calls to the container but makes an assumption
            // about dependency lifetimes.

            var terrain = _terrainFactory.Create(radius);
            var renderer = CreateRenderer(radius);
            var generator = Bootstrapper.Container.GetInstance<IHeightGenerator>();
            var statistics = Bootstrapper.Container.GetInstance<Statistics>();

            var planet = new Planet(location, radius, terrain, renderer, generator, _settings, statistics);

            return planet;
        }

        IPlanetRenderer CreateRenderer(double radius)
        {
            // TODO: put this in a separate factory class
            var contentManager = Bootstrapper.Container.GetInstance<ContentManager>();
            var graphicsDevice = Bootstrapper.Container.GetInstance<GraphicsDevice>();

            return new PlanetRenderer(radius, contentManager, graphicsDevice);
        }
    }
}
