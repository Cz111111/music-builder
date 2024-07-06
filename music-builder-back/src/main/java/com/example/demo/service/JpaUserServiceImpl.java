package com.example.demo.service;


import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import jakarta.annotation.Resource;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class JpaUserServiceImpl implements  JpaUserService {
    @Resource
    private UserRepository userRepository;

    @Autowired
    private PasswordEncoder passwordEncoder;


    public User findByUsername(String username){
        List<User> users=userRepository.findByUsername(username);
        if(users.size()>0) {
            return users.get(0);
        }
        return null;
    }

    public User insertUser(User user) {
        user.setPassword(passwordEncoder.encode(user.getPassword()));
        return userRepository.save(user);
    }

    public User updateUser(User user){
        return userRepository.save(user);
    }

    public void deleteUser(User user){
        userRepository.delete(user);
    }
}
