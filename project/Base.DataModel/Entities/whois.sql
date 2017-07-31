-- phpMyAdmin SQL Dump
-- version 4.4.15.1
-- http://www.phpmyadmin.net
--
-- Host: mysql552.umbler.com
-- Generation Time: 30-Jul-2017 às 12:35
-- Versão do servidor: 5.6.36-log
-- PHP Version: 5.4.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `whois`
--
CREATE DATABASE IF NOT EXISTS `whois` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `whois`;

-- --------------------------------------------------------

--
-- Estrutura da tabela `pesquisa`
--

CREATE TABLE IF NOT EXISTS `pesquisa` (
  `id` int(9) NOT NULL,
  `dthrpesquisa` datetime NOT NULL,
  `conteudo` varchar(30) NOT NULL,
  `iprequisicao` varchar(30) NOT NULL,
  `idsite` int(9) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=127 DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `site`
--

CREATE TABLE IF NOT EXISTS `site` (
  `id` int(9) NOT NULL,
  `dominio` varchar(60) NOT NULL,
  `hospedagem` varchar(100) NOT NULL,
  `ip` varchar(30) NOT NULL,
  `titular` varchar(100) DEFAULT NULL,
  `responsavel` varchar(100) DEFAULT NULL,
  `dtregistro` date NOT NULL,
  `dtexpiracao` date NOT NULL,
  `whois` longtext NOT NULL
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estrutura da tabela `site_nserver`
--

CREATE TABLE IF NOT EXISTS `site_nserver` (
  `idsite` int(9) NOT NULL,
  `dns` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `pesquisa`
--
ALTER TABLE `pesquisa`
  ADD PRIMARY KEY (`id`),
  ADD KEY `pesquisa_ibfk_1` (`idsite`);

--
-- Indexes for table `site`
--
ALTER TABLE `site`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dominio` (`dominio`);

--
-- Indexes for table `site_nserver`
--
ALTER TABLE `site_nserver`
  ADD PRIMARY KEY (`idsite`,`dns`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `pesquisa`
--
ALTER TABLE `pesquisa`
  MODIFY `id` int(9) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=127;
--
-- AUTO_INCREMENT for table `site`
--
ALTER TABLE `site`
  MODIFY `id` int(9) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=72;
--
-- Constraints for dumped tables
--

--
-- Limitadores para a tabela `pesquisa`
--
ALTER TABLE `pesquisa`
  ADD CONSTRAINT `pesquisa_ibfk_1` FOREIGN KEY (`idsite`) REFERENCES `site` (`id`);

--
-- Limitadores para a tabela `site_nserver`
--
ALTER TABLE `site_nserver`
  ADD CONSTRAINT `fk_site_nserver` FOREIGN KEY (`idsite`) REFERENCES `site` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
