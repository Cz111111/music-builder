package com.example.demo.service;

import com.example.demo.model.LoginUser;
import com.example.demo.model.User;
import jakarta.annotation.Resource;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetailsService;
import  org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

@Service
public class UserDetailServiceImpl implements UserDetailsService {
    @Autowired
    private JpaUserService jpaUserService;
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        User user = jpaUserService.findByUsername(username);
        if(Objects.isNull(user)){
            throw new UsernameNotFoundException("用户名或密码错误");
        }

  //      List<String> list = new ArrayList<>();
   //     list.add("select");
   //     list.add("delete");

        return new LoginUser(user/*,list*/);
    }
}
