package com.example.digitalsignatureapi.controllers;

import com.example.digitalsignatureapi.services.contracts.VerificationService;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class VerificationController {

        private final VerificationService verificationService;

        public VerificationController(VerificationService verificationService) {
                this.verificationService = verificationService;
        }
}
