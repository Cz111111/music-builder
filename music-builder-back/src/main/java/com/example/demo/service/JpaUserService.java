package com.example.demo.service;

import com.example.demo.model.User;

public interface JpaUserService {
    User findByUsername(String username);
    User insertUser(User user);
    User updateUser(User user);
    void deleteUser(User user);
}
