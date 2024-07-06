package com.example.demo.model;

import com.alibaba.fastjson.annotation.JSONField;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;


@Data
@NoArgsConstructor
//@AllArgsConstructor
public class LoginUser implements UserDetails {
    //权限列表
 //   private List<String> list;

    private User user;

 //   @JSONField(serialize=false)
 //   List<SimpleGrantedAuthority> authorities;


    public LoginUser(User user/*, List<String> list*/) {
        this.user = user;
 //       this.list = list;
    }

    @Override
    public Collection<? extends GrantedAuthority> getAuthorities() {
        /*
        if(authorities != null) {
            return authorities;
        }
        authorities = new ArrayList<>();
        for(String item : list) {
            SimpleGrantedAuthority authority = new SimpleGrantedAuthority(item);
            authorities.add(authority);
        }
        return authorities;*/
        return null;
    }

    @Override
    public String getPassword() {
        return user.getPassword();
    }

    @Override
    public String getUsername() {
        return user.getUsername();
    }

    @Override
    public boolean isAccountNonExpired() {
        return true;
    }

    @Override
    public boolean isAccountNonLocked() {
        return true;
    }
    @Override
    public boolean isCredentialsNonExpired() {
        return true;
    }
    @Override
    public boolean isEnabled() {
        return true;
    }

}
