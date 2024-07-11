package com.example.demo.model;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;


@AllArgsConstructor
@Data
@NoArgsConstructor
public class SongId implements Serializable {
    private String username;
    private String songword;
    // 需要实现equals和hashCode方法
    // 需要实现构造函数
}
