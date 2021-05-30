package com.example.digitalsignatureapi.controllers;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping(path = "${v1API}")
public class PingController {

    @GetMapping("/ping")
    public String ping() {
        return "PONG";
    }
}