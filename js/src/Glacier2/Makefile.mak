# **********************************************************************
#
# Copyright (c) 2003-2014 ZeroC, Inc. All rights reserved.
#
# This copy of Ice is licensed to you under the terms described in the
# ICE_LICENSE file included in this distribution.
#
# **********************************************************************

top_srcdir	= ..\..

LIBNAME = Glacier2

MODULES = $(LIBNAME)

GEN_SRCS = Metrics.js \
	PermissionsVerifier.js \
	PermissionsVerifierF.js \
	Router.js \
	RouterF.js \
	Session.js \
	SSLInfo.js

SDIR		= $(slicedir)\Glacier2

SRCS		= $(GEN_SRCS)
INSTALL_SRCS	= Glacier2.js $(GEN_SRCS)

!include $(top_srcdir)\config\Make.rules.mak.js

SLICE2JSFLAGS	= $(SLICE2JSFLAGS) --ice -I"$(slicedir)" --icejs

MODULEDIR       = $(install_moduledir)\$(LIBNAME)

install:: all
        @if not exist $(MODULEDIR) \
            @echo "Creating $(MODULEDIR)" && \
            mkdir "$(MODULEDIR)"
        @for %i in ( $(INSTALL_SRCS) ) do \
            copy %i "$(MODULEDIR)"
