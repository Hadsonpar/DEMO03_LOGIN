USE [BD_SEGURIDAD]
GO
/****** Object:  StoredProcedure [dbo].[GET_SEG_USUARIO]    Script Date: 9/11/2020 00:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ====================================================
-- Author:		HADSON PAREDES CORDOVA
-- Create date: 11/8/2020
-- Description:	USUARIO, CONTRASENA
-- ====================================================
ALTER PROCEDURE [dbo].[GET_SEG_USUARIO]
	@usuario VARCHAR	(50),
	@contrasena VARCHAR	(250)
AS
BEGIN
    SELECT usuario, contrasena
    FROM   Usuario 
	WHERE usuario = @usuario AND contrasena = @contrasena
END
